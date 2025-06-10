using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportifyApi.Controllers;
using Xunit;

namespace SportifyApi.Test.Controllers
{
    public class UploadControllerTests
    {
        private string CreateTempWebRoot(out string uploadsPath)
        {
            var tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            uploadsPath = Path.Combine(tempRoot, "uploads");
            Directory.CreateDirectory(uploadsPath);
            return tempRoot;
        }

        private IFormFile CreateFakeImage(string fileName = "test.jpg", string content = "fake image data")
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "image", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

        [Fact]
        public async Task UploadImage_ReturnsBadRequest_WhenImageIsNull()
        {
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns("wwwroot");

            var controller = new UploadController(envMock.Object);

            var result = await controller.UploadImage(null!, null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No image uploaded.", badRequest.Value);
        }

        [Fact]
        public async Task UploadImage_ReturnsImageUrl_WhenSuccessful()
        {
            var webRoot = CreateTempWebRoot(out var uploadsPath);
            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(webRoot);

            var controller = new UploadController(envMock.Object);
            var file = CreateFakeImage();

            var result = await controller.UploadImage(file, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("/uploads/", okResult.Value!.ToString());
        }

        [Fact]
        public async Task UploadImage_DeletesOldImage_IfExists()
        {
            var webRoot = CreateTempWebRoot(out var uploadsPath);
            var oldFileName = "old.png";
            var oldFilePath = Path.Combine(uploadsPath, oldFileName);
            await File.WriteAllTextAsync(oldFilePath, "old image content");

            Assert.True(File.Exists(oldFilePath));

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(webRoot);

            var controller = new UploadController(envMock.Object);
            var newImage = CreateFakeImage("new.png");

            var result = await controller.UploadImage(newImage, $"/uploads/{oldFileName}");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.False(File.Exists(oldFilePath)); // ✅ old file should be deleted
            Assert.Contains("/uploads/", ok.Value!.ToString());
        }
    }
}