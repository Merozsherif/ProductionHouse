using Microsoft.AspNetCore.Http;

namespace ProductionHouse.API.DTOs
{
    public class AddProjectRequest
    {
        public int CategoryId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        // Cover
        public IFormFile CoverImage { get; set; } = null!;

        // Gallery
        public List<IFormFile> GalleryImages { get; set; } = new();

        // English Content Only
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}