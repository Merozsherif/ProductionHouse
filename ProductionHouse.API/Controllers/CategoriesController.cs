using Microsoft.AspNetCore.Mvc;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Responses;

namespace ProductionHouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ===================== GET ALL =====================
        [HttpGet]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return Ok(new ApiResponse<CategoryDto>(
                true,
                "Category retrieved successfully.",
                category
            ));
        }

        // ===================== CREATE =====================
        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryDto dto)
        {
            await _categoryService.AddAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Category created successfully."
            ));
        }

        // ===================== UPDATE =====================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
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
        [HttpDelete("{id}")]
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