using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO.Region
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code must have minimun of 3 characters")]
        [MaxLength(3,ErrorMessage ="Code mus have maxiumum of 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage ="Name must have maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
