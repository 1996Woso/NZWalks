using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.DTO.Image;
using NZWalks.API.Models.DTO.Region;
using NZWalks.API.Models.DTO.Walk;

namespace NZWalks.API.Mappings
{
    public class AutoMaperProfiles: Profile
    {
        public AutoMaperProfiles()
        {
            //Mappings for Region

            // CreateMap<Source,Destination>()
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDTO,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDTO, Region>().ReverseMap();
            //Mappings for Walks
            CreateMap<AddWalkRequestDTO, Walk>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();   
            CreateMap<UpdateWalkRequestDTO, Walk>().ReverseMap();
            //Mappings for Difficulty
            CreateMap<Difficulty,DifficultyDTO>().ReverseMap();
          
           
        }
    }
}
