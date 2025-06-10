using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;

namespace SportifyApi.Test.Helpers
{
    public static class TestHelpers
    {
        public static AppDbContext CreateInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }
    }
}
