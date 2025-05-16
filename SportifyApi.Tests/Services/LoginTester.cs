using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Models;
using Microsoft.AspNetCore.Identity;
using Xunit;
using FluentAssertions;

namespace SportifyApi.Tests.Services
{
    public class LoginTester
    {
        private async Task<AppDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                Name = "TestUser",
                Email = "testuser@example.com",
                UserType = "user"
            };
            user.Password = passwordHasher.HashPassword(user, "1234");

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_ShouldPass()
        {
            var context = await GetInMemoryDbContextAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "testuser@example.com");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, "1234");

            result.Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ShouldFail()
        {
            var context = await GetInMemoryDbContextAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "testuser@example.com");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, "wrongpass");

            result.Should().Be(PasswordVerificationResult.Failed);
        }

        [Fact]
        public async Task Login_WithInvalidEmail_ShouldReturnNull()
        {
            var context = await GetInMemoryDbContextAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "invalid@example.com");

            user.Should().BeNull();
        }
    }
}
