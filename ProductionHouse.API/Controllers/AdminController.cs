using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Responses;
using ProductionHouse.Infrastructure.Data;

namespace ProductionHouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Stats")]
        [Authorize] // 🔒 محمي للـ Admin فقط باستخدام الـ JWT Token
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalProjects = await _context.Projects.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                FeaturedProjectsCount = await _context.Projects.CountAsync(p => p.IsFeatured)
            };

            return Ok(new ApiResponse<DashboardStatsDto>
            {
                Success = true,
                Message = "Dashboard statistics retrieved successfully.",
                Data = stats
            });
        }
    }
}
