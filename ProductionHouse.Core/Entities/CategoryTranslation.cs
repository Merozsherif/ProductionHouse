using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Entities
{
    public class CategoryTranslation
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string LanguageCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public Category Category { get; set; } = null!;
    }
}
