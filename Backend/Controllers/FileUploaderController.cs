using System.Net.Mime;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/uploadImage")]
[ApiController]
public class FileUploaderController : ControllerBase
{
    [HttpPost("{id}")]
    public async Task<IActionResult> UploadImage([FromForm] ImageDTO img, string id)
    {
        if (img.Image == null || img.Image.Length == 0)
        {
            return BadRequest("Image is required.");
        }

        try
        {
            Image image = new Image { FileName = img.FileName };
            using (var binaryReader = new BinaryReader(img.Image.OpenReadStream()))
            {
                image.Picture = binaryReader.ReadBytes((int)img.Image.Length);
            }

            var uploadsFolder = Path.Combine("uploads", "images", id);
            Directory.CreateDirectory(uploadsFolder); // Create folder if it doesn't exist
            var filePath = Path.Combine(uploadsFolder, img.FileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await img.Image.CopyToAsync(stream);
            }

            Console.WriteLine("Image successfully uploaded.");

            return Ok(new { Message = "Image uploaded successfully.", Path = filePath });
            //TODO
            //Send image to docker container and run the script inside, it should return JSON with Product class items which then we add to db
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); //change to logger later
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }
}