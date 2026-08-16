using ProductionHouse.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.DTOs
{
    public class ProjectQueryDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public bool? IsPublished { get; set; }

        public bool? IsFeatured { get; set; }

        public ProjectSortBy? SortBy { get; set; }


        public bool Desc { get; set; }
    }
}
