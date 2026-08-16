using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalProjects { get; set; }
        public int TotalCategories { get; set; }
        public int FeaturedProjectsCount { get; set; }
    }
}
