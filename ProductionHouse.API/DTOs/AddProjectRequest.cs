using Microsoft.AspNetCore.Http;

namespace ProductionHouse.API.DTOs
{
    public class AddProjectRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }   // بدل CategoryName
        public string ClientName { get; set; } = string.Empty;
        public IFormFile CoverImage { get; set; } = null!;
        public List<IFormFile> GalleryImages { get; set; } = new();

        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
    }
}