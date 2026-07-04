using System.Collections.Generic;

namespace ProductionHouse.Core.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string CoverImage { get; set; } = string.Empty;

        public List<ProjectTranslationDto> Translations { get; set; } = new();
    }

    public class AddProjectDto
    {
        public int CategoryId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string CoverImage { get; set; } = string.Empty;

        public List<ProjectTranslationDto> Translations { get; set; } = new();
    }

    public class UpdateProjectDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string CoverImage { get; set; } = string.Empty;

        public List<ProjectTranslationDto> Translations { get; set; } = new();
    }

    public class ProjectTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}