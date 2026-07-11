using ProductionHouse.Core.Enums;
using System.Collections.Generic;

namespace ProductionHouse.Core.DTOs
{
    public class AddCategoryDto
    {

        public List<AddCategoryTranslationDto> Translations { get; set; } = new();
    }

    public class UpdateCategoryDto
    {
        public int Id { get; set; }



        public List<AddCategoryTranslationDto> Translations { get; set; } = new();
    }

    public class CategoryDto
    {
        public int Id { get; set; }


        public List<CategoryTranslationDto> Translations { get; set; } = new();
    }

    public class AddCategoryTranslationDto
    {
        public LanguageCode LanguageCode { get; set; } 

        public string Name { get; set; } = string.Empty;
    }

    public class CategoryTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}