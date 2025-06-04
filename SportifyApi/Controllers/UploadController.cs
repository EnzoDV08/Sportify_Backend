using Microsoft.AspNetCore.Mvc;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }
    

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image, [FromQuery] string? oldImageUrl = null)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { error = "No file uploaded." });

            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            // 🗑️ Delete old image if provided
            if (!string.IsNullOrWhiteSpace(oldImageUrl))
            {
                try
                {
                    var oldFileName = Path.GetFileName(new Uri(oldImageUrl).AbsolutePath);
                    var oldFilePath = Path.Combine(uploadsDir, oldFileName);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }
                catch { /* silent fail */ }
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
            return Ok(new { imageUrl });
        }
    }
}


