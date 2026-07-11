using Microsoft.AspNetCore.Http;

namespace ProductionHouse.API.DTOs
{
    public class AddCategoryRequest
    {
        public IFormFile Image { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
    }
}