namespace ProductionHouse.API.DTOs
{
    public class UploadGalleryRequest
    {
        public List<IFormFile> Images { get; set; } = new();
    }
}
