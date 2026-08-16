using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Entities
{
    public class Project
    {
        public string? ImageFolder { get; set; }
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string CoverImage { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Category Category { get; set; } = null!;

        public List<ProjectImage> Images { get; set; } = new();

        public List<ProjectTranslation> Translations { get; set; } = new();


        public bool IsPublished { get; set; } = true;

        public bool IsFeatured { get; set; }

        public int DisplayOrder { get; set; }
    }
}