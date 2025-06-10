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
