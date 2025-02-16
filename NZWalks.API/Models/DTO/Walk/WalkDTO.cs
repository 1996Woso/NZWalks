using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Models.DTO.Walk
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
  

        //Navigation properties
        public RegionDTO Region { get; set; }
        public DifficultyDTO Difficulty { get; set; }
    }
}
