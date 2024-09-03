namespace Backend.Models;

public class ImageDTO
{
    public string FileName { get; set; }

    public IFormFile Image { get; set; }
}