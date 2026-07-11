using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }

     

        // Navigation
        public List<Project> Projects { get; set; } = new();

        public List<CategoryTranslation> Translations { get; set; } = new();


        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }
    }
}
