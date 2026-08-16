using Microsoft.AspNetCore.Http;
using ProductionHouse.Core.Enums;
using System.Collections.Generic;

namespace ProductionHouse.Core.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;

        public string CoverImage { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public List<ProjectImageDto> GalleryImages { get; set; } = new();

        public List<ProjectTranslationDto> Translations { get; set; } = new();
    }

    public class AddProjectDto
    {
        public int CategoryId { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public IFormFile CoverImage { get; set; } = null!;
        public List<IFormFile> GalleryImages { get; set; } = new();

        public List<ProjectTranslationDto> Translations { get; set; } = new();
    }

    public class UpdateProjectDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string? CoverImage { get; set; }

        public List<string> GalleryImages { get; set; } = new();
        public List<int> DeletedGalleryImages { get; set; } = new();

        public List<ProjectTranslationDto> Translations { get; set; } = new();
    }
    public class ProjectTranslationDto
    {

        public LanguageCode LanguageCode { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

        public class ProjectImageDto
        {
            public int Id { get; set; }

            public string ImageUrl { get; set; } = string.Empty;
        }

}