using Microsoft.AspNetCore.Http;

namespace ProductionHouse.API.DTOs
{
    public class ChangeCoverRequest
    {
        public IFormFile CoverImage { get; set; } = null!;
    }
}