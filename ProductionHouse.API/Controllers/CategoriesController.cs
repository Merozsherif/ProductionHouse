using Microsoft.AspNetCore.Mvc;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Responses;

namespace ProductionHouse.API.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ===================== GET ALL =====================
        // URL: /api/Categories/Get-All
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(new ApiResponse<List<CategoryDto>>(
                true,
                "Categories retrieved successfully.",
                categories
            ));
        }

        // ===================== GET BY ID =====================
        // URL: /api/Categories/Get-By-Id/5
        [HttpGet("Get-By-Id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return Ok(new ApiResponse<CategoryDto>(
                true,
                "Category retrieved successfully.",
                category
            ));
        }

        // ===================== CREATE (ADD) =====================
        // URL: /api/Categories/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddCategoryDto dto)
        {
            await _categoryService.AddAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Category created successfully."
            ));
        }

        // ===================== UPDATE =====================
        // URL: /api/Categories/Update/5
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ApiResponse<string>(
                    false,
                    "Invalid category id."
                ));
            }

            await _categoryService.UpdateAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Category updated successfully."
            ));
        }

        // ===================== DELETE =====================
        // URL: /api/Categories/Delete/5
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok(new ApiResponse<string>(
                true,
                "Category deleted successfully."
            ));
        }
    }
}