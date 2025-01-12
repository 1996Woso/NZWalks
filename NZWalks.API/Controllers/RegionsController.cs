using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext context;
        private readonly IRegionRepository regionRepos;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext context, IRegionRepository regionRepos, IMapper mapper)
        {
            this.context = context;
            this.regionRepos = regionRepos;
            this.mapper = mapper;
        }
        //Get All Regions
        //Get: http://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from db - Domain models
            var regionsDM =  await regionRepos.GetAllAsync();
           //Map Domain models to DTOs(Data Transfer Objects)
     
            var regionsDTO = mapper.Map<List<RegionDTO>>(regionsDM);
            //Return DTOs
            return Ok(regionsDTO);
        }
        //Get region by id
        //Get: http://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //Rgion Domain model
            var regionDM = await regionRepos.GetByIdAsync(id);
            //var region = context.Regions.Find(id);
            if(regionDM == null)
            {
                return NotFound();
            }
            //Map/Convert Region DM to TDO
            var regionsDTO = mapper.Map<RegionDTO>(regionDM);
       
            //Rerurn DTO Back to client
            return Ok(regionsDTO);
        }

        //POST methos to add new region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map or convert DTO to DM
            var regionDM = mapper.Map<Region>(addRegionRequestDTO);
            //Use DM to create Region
            await regionRepos.CreateAsync(regionDM);
            //Map DM back to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDM);
            return CreatedAtAction(nameof(GetAll), new { regionDTO.Id },regionDTO);
        }
        //Update region
        //PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Map DTO to DM
            var regionDM = mapper.Map<Region>(updateRegionRequestDTO);
            //Check if region exists
            regionDM = await regionRepos.UpdateAsync(id, regionDM);
            if(regionDM == null)
            {
                return NotFound();
            }
            //Convert DM to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDM);
            //Region regionDTO to client
            return Ok(regionDTO);
            
        }
        //
        //Delete region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDM = await regionRepos.DeleteAsync(id);
            if(regionDM == null)
            {
                return NotFound();
            }
            //Optional-return deleted region
            //Map DM to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDM);
            
            return Ok(regionDTO);//Or return Ok();
        }


    }
}
