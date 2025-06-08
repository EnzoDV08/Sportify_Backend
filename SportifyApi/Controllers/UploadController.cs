using Microsoft.AspNetCore.Mvc;

namespace SportifyApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public UploadController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile image, [FromForm] string? oldImageUrl)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image uploaded.");
        }

        // ✅ Get the uploads folder path safely
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // ✅ Delete old image if a valid one was supplied
        if (!string.IsNullOrEmpty(oldImageUrl))
        {
            var oldFileName = Path.GetFileName(oldImageUrl);
            var oldFilePath = Path.Combine(uploadsFolder, oldFileName);

            if (System.IO.File.Exists(oldFilePath))
                System.IO.File.Delete(oldFilePath);
        }

        var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var newFilePath = Path.Combine(uploadsFolder, newFileName);

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var relativeUrl = $"/uploads/{newFileName}";
        return Ok(new { imageUrl = relativeUrl });
    }
}
}


