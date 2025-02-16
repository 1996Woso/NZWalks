using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Walk;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepos;

        public WalksController(IMapper mapper, IWalkRepository walkRepos)
        {
            this.mapper = mapper;
            this.walkRepos = walkRepos;
        }
        //Create walk
        //POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalksRequestDTO)
        {
            //Map DTO to DM
            var walkDM = mapper.Map<Walk>(addWalksRequestDTO);
            await walkRepos.CreateAsync(walkDM);
            //Map DM to DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDM);
            return Ok(walkDTO);
        }
        //Get All Walks
        //GET: /api/walks?filterOn =Name&filterQuery=Track&&sortBy=Name&&isAscending=true&pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //Get walks from db
            var walksDM = await walkRepos.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true,pageNumber, pageSize);
            //Create an exception
            throw new Exception("This is exception");
            //Map DM to DTO
            var walksDTO = mapper.Map<List<WalkDTO>>(walksDM);
            return Ok(walksDTO);
        }
        //Get walk by id
        //GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Walk DM
            var walkDM = await walkRepos.GetByIdAsync(id);
            if (walkDM == null) return NotFound();
            //Map Walk DM to Walk DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDM);
            return Ok(walkDTO);
        }
        //Updtate Walk
        //PUT: api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            //Map DTO to DM
            var walkDM = mapper.Map<Walk>(updateWalkRequestDTO);
            //Check if walk exists
            walkDM = await walkRepos.UpdateAsync(id, walkDM);
            if (walkDM == null) return NotFound();
            //Map DM to DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDM);
            return Ok(walkDTO);
        }
        //Delete walk
        //DELETE: /api/walk/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDM = await walkRepos.DeleteAsync(id);
            if (walkDM == null) return NotFound();
            //Map DM to DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDM);
            return Ok(walkDTO);
        }


    }
}
