using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO.Auth;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepos;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepos)
        {
            this.userManager = userManager;
            this.tokenRepos = tokenRepos;
        }
        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var user = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };
            var result = await userManager.CreateAsync(user,registerRequestDTO.Password);
            if (result.Succeeded)
            {
                //Add Role(s) to the user
                if(registerRequestDTO.Roles != null)
                {
                    result = await userManager.AddToRolesAsync(user, registerRequestDTO.Roles);
                }
                return Ok("User is successfully registered.");
            }
            return BadRequest("Error has occured while registering");
        }
        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if(user != null)
            {
                //Check if password is correct
                var result = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (result)
                {
                    //Get user roles
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //Create Token
                        var jwtToken = await tokenRepos.CreateJWTTokenAsync(user, roles.ToList());

                        var jwtTokenDTO = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(jwtTokenDTO);
                    }
                }
                else
                {
                    return BadRequest("Incorrect password.");
                }
            }
            return BadRequest($"Invalid Log in, {loginRequestDTO.Username} does not exists.");
        }
    }
}
