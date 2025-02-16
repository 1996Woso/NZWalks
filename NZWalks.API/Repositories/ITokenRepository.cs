using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        Task<string> CreateJWTTokenAsync(IdentityUser user, List<string> roles);
    }
}
