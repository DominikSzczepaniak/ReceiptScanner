﻿using System.Net.Mime;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/uploadImage")]
[ApiController]
public class FileUploaderController : ControllerBase
{
    [HttpPost("{id}")]
    public Task<IActionResult> UploadImage([FromForm] ImageDTO img, string id)
    {
        Image image = new Image { FileName = img.FileName };
        byte[] imageData = null;
        using (var binaryReader = new BinaryReader(img.Image.OpenReadStream()))
        {
            imageData = binaryReader.ReadBytes((int)img.Image.Length);
        }
        image.Picture = imageData;
        Console.WriteLine("Correctly updated");
        //TODO Create sequence in database for image id 
        // Add image to folder images with given id 
        // Run docker script on this image 
        return Task.FromResult<IActionResult>(Ok());
    }
} 