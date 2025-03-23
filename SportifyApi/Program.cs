using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Load environment variables
DotNetEnv.Env.Load(); // Make sure to install DotNetEnv

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get password from .env
var aivenPassword = Environment.GetEnvironmentVariable("AIVEN_DB_PASSWORD");

// Replace __AIVEN_DB_PASSWORD__ in appsettings
var connectionString = builder.Configuration
    .GetConnectionString("LiveAivenConnection")!
    .Replace("__AIVEN_DB_PASSWORD__", aivenPassword);

// Connect to PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

