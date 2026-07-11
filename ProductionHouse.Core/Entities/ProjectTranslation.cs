using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Enums;

public class ProjectTranslation
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public LanguageCode LanguageCode { get; set; }

    // Content
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // SEO
    public string Slug { get; set; } = string.Empty;

    public string MetaTitle { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public string MetaKeywords { get; set; } = string.Empty;

    // Social Sharing
    public string OgTitle { get; set; } = string.Empty;

    public string OgDescription { get; set; } = string.Empty;

    public string AltText { get; set; } = string.Empty;

    public Project Project { get; set; } = null!;
}