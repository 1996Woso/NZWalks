using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext context;
        public RegionsController(NZWalksDbContext context)
        {
            this.context = context;
        }
        //Get All Regions
        //Get: http://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from db - Domain models
            var regionsDM = context.Regions.ToList();
            //Map Domain models to DTOs
            var regionsDTO = new List<RegionDTO>();
            foreach(var region in regionsDM)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            //Return DTOs
            return Ok(regionsDTO);
        }
        //Get region by id
        //Get: http://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            //Rgion Domain model
            var regionDM = context.Regions.FirstOrDefault(x => x.Id == id);
            //var region = context.Regions.Find(id);
            if(regionDM == null)
            {
                return NotFound();
            }
            //Map/Convert Region DM to TDO
            var regionsDTO = new RegionDTO
            {
                Id = regionDM.Id,
                Name = regionDM.Name,
                Code = regionDM.Code,
                RegionImageUrl = regionDM.RegionImageUrl
            };
            //Rerurn DTO Back to client
            return Ok(regionsDTO);
        }

        //POST methos to add new region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO region)
        {
            //Map or convert DTO to DM
            var regionDM = new Region
            {
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
                Name = region.Name
            };
            //Use DM to create Region
            context.Regions.Add(regionDM);
            context.SaveChanges();
            //Map DM back to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDM.Id,
                Code = regionDM.Code,
                Name = regionDM.Name,
                RegionImageUrl = regionDM.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetAll), new { regionDTO.Id },regionDTO);
        }
        //Update region
        //PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO region)
        {
            //Check if region exists
            var regionDM = context.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDM == null)
            {
                return NotFound();
            }
            //Map DTO to DM
            regionDM.RegionImageUrl = region.RegionImageUrl;
            regionDM.Name = region.Name;
            regionDM.Code = region.Code;
            context.SaveChanges();
            //Map DM to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDM.Id,
                Code = regionDM.Code,
                Name = regionDM.Name,
                RegionImageUrl = regionDM.RegionImageUrl
            };
            //Region regionDTO to client
            return Ok(regionDTO);
            
        }
        //
        //Delete region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            //Check if region exists
            var regionDM = context.Regions.FirstOrDefault(x => x.Id==id);
            if(regionDM == null)
            {
                return NotFound();
            }
            context.Regions.Remove(regionDM);
            context.SaveChanges();
            //Optional-return deleted region
            //Map DM to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDM.Id,
                Code = regionDM.Code,
                Name = regionDM.Name,
                RegionImageUrl = regionDM.RegionImageUrl
            };
            
            return Ok(regionDTO);//Or return Ok();
        }


    }
}
