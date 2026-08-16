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
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}