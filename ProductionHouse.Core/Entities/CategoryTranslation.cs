using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Enums;

public class CategoryTranslation
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public LanguageCode LanguageCode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string MetaTitle { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public Category Category { get; set; } = null!;
}