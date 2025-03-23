using System;
using Microsoft.EntityFrameworkCore;


namespace SportifyApi.Data;

public class AppDbContext : DbContext
{
    // constructor - use all the base context options for our db context
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}


    // TODO: add all my other tables here too.
       //override the on model creating method
    // this is where we specify the relationships between our DB
}

// STEPS TO CREATE A TABLE IN MY DB:
// 1. Create a model to represent the structure/columns of your table
// 2. Add the table in our db context
// 3. Create a migration specifying the changes made (dotnet ef migrations add InitialCreation)
// 4. database update to sync (dotnet ef database update )
