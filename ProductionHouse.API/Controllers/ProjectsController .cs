using Microsoft.AspNetCore.Mvc;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Responses;

namespace ProductionHouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // ===================== GET ALL =====================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();

            return Ok(new ApiResponse<List<ProjectDto>>(
                true,
                "Projects retrieved successfully.",
                projects
            ));
        }

        // ===================== GET BY ID =====================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);

            return Ok(new ApiResponse<ProjectDto>(
                true,
                "Project retrieved successfully.",
                project
            ));
        }

        // ===================== CREATE =====================
        [HttpPost]
        public async Task<IActionResult> Add(AddProjectDto dto)
        {
            await _projectService.AddAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Project created successfully."
            ));
        }

        // ===================== UPDATE =====================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ApiResponse<string>(
                    false,
                    "Invalid project id."
                ));
            }

            await _projectService.UpdateAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Project updated successfully."
            ));
        }

        // ===================== DELETE =====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteAsync(id);

            return Ok(new ApiResponse<string>(
                true,
                "Project deleted successfully."
            ));
        }
    }
}