using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Models;
using SportifyApi.Services;
using System.Threading.Tasks;
using Xunit;

namespace SportifyApi.Test.Services
{
    public class FriendServiceTests
    {
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "FriendDb_" + Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task SendRequest_ShouldAddFriend()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            var dto = new FriendRequestDto { SenderId = 1, ReceiverId = 2 };
            await service.SendRequestAsync(dto);

            var friend = await context.Friends.FirstOrDefaultAsync();
            Assert.NotNull(friend);
            Assert.Equal("pending", friend.Status);
        }

        [Fact]
        public async Task AcceptRequest_ShouldUpdateStatus()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            var friend = new Friend { UserId = 1, FriendId = 2, Status = "pending" };
            context.Friends.Add(friend);
            await context.SaveChangesAsync();

            await service.AcceptRequestAsync(friend.Id);

            var updated = await context.Friends.FindAsync(friend.Id);
            Assert.NotNull(updated);
            Assert.Equal("accepted", updated!.Status);
        }

        [Fact]
        public async Task GetMyFriendsAsync_ReturnsAcceptedFriends()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            // Arrange: add two users and a friend relationship
            context.Users.AddRange(
                new User { UserId = 1, Name = "Alice", Email = "alice@example.com" },
                new User { UserId = 2, Name = "Bob", Email = "bob@example.com" }
            );

            context.Profiles.Add(new Profile { UserId = 2, ProfilePicture = "bob.jpg", Bio = "Hi", FavoriteSports = "Soccer", TotalPoints = 100 });

            context.Friends.Add(new Friend { UserId = 1, FriendId = 2, Status = "accepted" });
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetMyFriendsAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("accepted", result[0].Status);
            Assert.Equal("Bob", result[0].User.Name);
        }

        [Fact]
        public async Task GetFriendRequestsAsync_ReturnsPendingRequests()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            context.Users.Add(new User { UserId = 1, Name = "Alice", Email = "alice@example.com" });
            context.Profiles.Add(new Profile { UserId = 1, Bio = "Hey there!" });

            context.Friends.Add(new Friend { UserId = 1, FriendId = 2, Status = "pending" });
            await context.SaveChangesAsync();

            var result = await service.GetFriendRequestsAsync(2);

            Assert.Single(result);
            Assert.Equal("pending", result[0].Status);
            Assert.Equal("Alice", result[0].User.Name);
        }

        [Fact]
        public async Task SearchUsersAsync_ReturnsMatchingUsersExceptSelf()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            context.Users.AddRange(
                new User { UserId = 1, Name = "Current User", Email = "me@example.com" },
                new User { UserId = 2, Name = "Bob Searchable", Email = "bob@example.com" },
                new User { UserId = 3, Name = "Charlie", Email = "charlie@example.com" }
            );

            context.Profiles.Add(new Profile { UserId = 2, ProfilePicture = "bob.jpg", Bio = "Bio" });

            await context.SaveChangesAsync();

            var result = await service.SearchUsersAsync("Bob", 1);

            Assert.Single(result);
            Assert.Equal("Bob Searchable", result[0].User.Name);
            Assert.Equal("bob.jpg", result[0].Profile.ProfilePicture);
        }


        [Fact]
        public async Task RejectRequest_ShouldUpdateStatus()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            var friend = new Friend { UserId = 1, FriendId = 2, Status = "pending" };
            context.Friends.Add(friend);
            await context.SaveChangesAsync();

            await service.RejectRequestAsync(friend.Id);

            var updated = await context.Friends.FindAsync(friend.Id);
           Assert.NotNull(updated);
Assert.Equal("rejected", updated!.Status);

        }

        [Fact]
        public async Task DeleteFriend_ShouldRemoveEntry()
        {
            var context = GetInMemoryDb();
            var service = new FriendService(context);

            var friend = new Friend { UserId = 1, FriendId = 2, Status = "accepted" };
            context.Friends.Add(friend);
            await context.SaveChangesAsync();

            await service.DeleteFriendAsync(friend.Id);

            var deleted = await context.Friends.FindAsync(friend.Id);
            Assert.Null(deleted);
        }
    }
}