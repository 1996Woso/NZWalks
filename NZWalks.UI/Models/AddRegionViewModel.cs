using System.ComponentModel;

namespace NZWalks.UI.Models
{
    public class AddRegionViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        [DisplayName("Image URL")]
        public string RegionImageUrl { get; set; }
    }
}
