using System.Collections.Generic;

namespace ProductionHouse.Core.DTOs
{
    public class AddCategoryDto
    {
        public string Image { get; set; } = string.Empty;

        public List<AddCategoryTranslationDto> Translations { get; set; } = new();
    }

    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        public string Image { get; set; } = string.Empty;

        public List<AddCategoryTranslationDto> Translations { get; set; } = new();
    }

    public class CategoryDto
    {
        public int Id { get; set; }

        public string Image { get; set; } = string.Empty;

        public List<CategoryTranslationDto> Translations { get; set; } = new();
    }

    public class AddCategoryTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }

    public class CategoryTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}