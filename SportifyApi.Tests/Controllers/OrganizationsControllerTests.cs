using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SportifyApi.Controllers;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using Xunit;

namespace SportifyApi.Test.Controllers
{
    public class OrganizationsControllerTests
    {
        private readonly Mock<IOrganizationService> _mockOrgService;
        private readonly AppDbContext _context;

        public OrganizationsControllerTests()
        {
            _mockOrgService = new Mock<IOrganizationService>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "OrgTestDb")
                .Options;
            _context = new AppDbContext(options);
        }

        [Fact]
        public async Task Register_CreatesOrgAndProfile()
        {
            // Arrange
            var dto = new OrganizationDto { Name = "Test Org", Email = "test@org.com", Password = "pass123" };
            var createdOrg = new OrganizationDto { OrganizationId = 1, Name = "Test Org", Email = "test@org.com" };

            _mockOrgService.Setup(s => s.CreateAsync(dto, dto.Password)).ReturnsAsync(createdOrg);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            // Act
            var result = await controller.Register(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var value = Assert.IsType<OrganizationDto>(createdResult.Value);
            Assert.Equal("Test Org", value.Name);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenPasswordMissing()
        {
            var dto = new OrganizationDto { Name = "Test Org", Email = "test@org.com", Password = null };

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.Register(dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password is required.", badResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenValid()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("testpass");
            var org = new Organization
            {
                OrganizationId = 1,
                Name = "Org",
                Email = "test@org.com",
                Password = hashedPassword,
                Website = "https://org.com",
                ContactPerson = "John"
            };
            await _context.Organizations.AddAsync(org);
            await _context.SaveChangesAsync();

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var login = new LoginRequestDto { Email = "test@org.com", Password = "testpass" };

            var result = await controller.Login(login);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<OrganizationDto>(okResult.Value);

            Assert.Equal("Org", value.Name);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalid()
        {
            var controller = new OrganizationsController(_mockOrgService.Object, _context);
            var login = new LoginRequestDto { Email = "invalid@org.com", Password = "wrongpass" };

            var result = await controller.Login(login);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsAllOrganizations()
        {
            var orgs = new List<OrganizationDto>
            {
                new OrganizationDto { OrganizationId = 1, Name = "Org1", Email = "org1@test.com" },
                new OrganizationDto { OrganizationId = 2, Name = "Org2", Email = "org2@test.com" }
            };

            _mockOrgService.Setup(s => s.GetAllAsync()).ReturnsAsync(orgs);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<OrganizationDto>>(okResult.Value);
            Assert.Equal(2, ((List<OrganizationDto>)returned).Count);
        }

        [Fact]
        public async Task GetById_ReturnsOrg_WhenExists()
        {
            var org = new OrganizationDto { OrganizationId = 1, Name = "Org1", Email = "org@test.com" };

            _mockOrgService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(org);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.GetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<OrganizationDto>(okResult.Value);
            Assert.Equal(1, returned.OrganizationId);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _mockOrgService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((OrganizationDto?)null);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.GetById(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateOrganization_ReturnsNoContent_WhenUpdated()
        {
            var updateDto = new UpdateOrganizationDto { Name = "Updated Org" };
            _mockOrgService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(true);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.UpdateOrganization(1, updateDto);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateOrganization_ReturnsNotFound_WhenNotFound()
        {
            var updateDto = new UpdateOrganizationDto { Name = "Updated Org" };
            _mockOrgService.Setup(s => s.UpdateAsync(99, updateDto)).ReturnsAsync(false);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.UpdateOrganization(99, updateDto);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            _mockOrgService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            _mockOrgService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var controller = new OrganizationsController(_mockOrgService.Object, _context);

            var result = await controller.Delete(99);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
