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

        public string Title { get; set; } = string.Empty;
        public List<int> DeletedGalleryImages { get; set; } = new();
        public string Description { get; set; } = string.Empty;
    }
}