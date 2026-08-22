using Microsoft.AspNetCore.Http;

namespace ProductionHouse.API.DTOs
{
    public class UpdateProjectRequest
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public IFormFile? CoverImage { get; set; }
        public List<IFormFile>? GalleryImages { get; set; }
        public List<int> DeletedGalleryImages { get; set; } = new();

        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
    }
}