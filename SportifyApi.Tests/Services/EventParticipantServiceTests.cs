// using Moq;
// using Xunit;
// using SportifyApi.Models;
// using SportifyApi.Services;
// using SportifyApi.Data;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using System.Linq;

// namespace SportifyApi.Tests.Services
// {
//     public class EventParticipantServiceTests
//     {
//         private readonly DbContextOptions<AppDbContext> _options;

//         public EventParticipantServiceTests()
//         {
//             _options = new DbContextOptionsBuilder<AppDbContext>()
//                 .UseInMemoryDatabase(databaseName: $"EventParticipantTestDb_{System.Guid.NewGuid()}")
//                 .Options;
//         }

//         [Fact]
//         public async Task JoinEventAsync_Should_Add_New_Participant()
//         {
//             using var context = new AppDbContext(_options);
//             context.Users.Add(new User { UserId = 1, UserType = "user" });
//             context.Events.Add(new Event { EventId = 1, Title = "Event 1" });
//             await context.SaveChangesAsync();

//             var service = new EventParticipantService(context);
//             var result = await service.JoinEventAsync(1, 1);

//             Assert.True(result);
//             Assert.Single(context.EventParticipants);
//         }

//         [Fact]
//         public async Task ApproveRequestAsync_Should_Approve_When_Admin()
//         {
//             using var context = new AppDbContext(_options);
//             context.Users.Add(new User { UserId = 10, UserType = "admin" });
//             context.Events.Add(new Event { EventId = 5, CreatorUserId = 99 });
//             context.EventParticipants.Add(new EventParticipant { EventId = 5, UserId = 20, Status = "Pending" });
//             await context.SaveChangesAsync();

//             var service = new EventParticipantService(context);
//             var result = await service.ApproveRequestAsync(5, 20, 10);

//             Assert.True(result);
//             Assert.Equal("Approved", context.EventParticipants.First().Status);
//         }

//         [Fact]
//         public async Task RejectRequestAsync_Should_Reject_When_Creator()
//         {
//             using var context = new AppDbContext(_options);
//             context.Users.Add(new User { UserId = 50, UserType = "user" });
//             context.Events.Add(new Event { EventId = 9, CreatorUserId = 50 });
//             context.EventParticipants.Add(new EventParticipant { EventId = 9, UserId = 2, Status = "Pending" });
//             await context.SaveChangesAsync();

//             var service = new EventParticipantService(context);
//             var result = await service.RejectRequestAsync(9, 2, 50);

//             Assert.True(result);
//             Assert.Equal("Rejected", context.EventParticipants.First().Status);
//         }

//         [Fact]
//         public async Task GetPendingRequestsAsync_Should_Return_All_For_Admin()
//         {
//             using var context = new AppDbContext(_options);
//             context.Users.AddRange(
//                 new User { UserId = 99, UserType = "user" },
//                 new User { UserId = 100, UserType = "admin" }
//             );

//             var event1 = new Event { EventId = 1, Title = "Event 1", CreatorUserId = 99 };
//             var event2 = new Event { EventId = 2, Title = "Event 2", CreatorUserId = 99 };
//             context.Events.AddRange(event1, event2);
//             await context.SaveChangesAsync();

//             context.EventParticipants.AddRange(
//                 new EventParticipant { UserId = 2, Status = "Pending", EventId = 1 },
//                 new EventParticipant { UserId = 3, Status = "Pending", EventId = 2 }
//             );
//             await context.SaveChangesAsync();

//             var service = new EventParticipantService(context);
//             var results = await service.GetPendingRequestsAsync(100);

//             foreach (var r in results)
//             {
//                 Console.WriteLine($"[Admin] Found pending request: EventId={r.EventId}, UserId={r.UserId}, Status={r.Status}");
//             }

//             Assert.Equal(2, results.Count);
//         }

//         [Fact]
//         public async Task GetPendingRequestsAsync_Should_Filter_By_Creator_When_Not_Admin()
//         {
//             using var context = new AppDbContext(_options);
//             context.Users.AddRange(
//                 new User { UserId = 101, UserType = "user" },
//                 new User { UserId = 999, UserType = "user" }
//             );

//             var ownedEvent = new Event { EventId = 22, CreatorUserId = 101 };
//             var otherEvent = new Event { EventId = 33, CreatorUserId = 999 };
//             context.Events.AddRange(ownedEvent, otherEvent);
//             await context.SaveChangesAsync();

//             context.EventParticipants.AddRange(
//                 new EventParticipant { UserId = 3, Status = "Pending", EventId = 22 },
//                 new EventParticipant { UserId = 4, Status = "Pending", EventId = 33 }
//             );
//             await context.SaveChangesAsync();

//             var service = new EventParticipantService(context);
//             var results = await service.GetPendingRequestsAsync(101);

//             foreach (var r in results)
//             {
//                 Console.WriteLine($"[Creator] Found pending request: EventId={r.EventId}, UserId={r.UserId}, Status={r.Status}, CreatorId={r.Event?.CreatorUserId}");
//             }

//             Assert.Single(results);
//             Assert.Equal(22, results[0].EventId);
//         }
//     }
// }
