using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Services;
using SportifyApi.Interfaces;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Load environment variables from .env
Env.Load();

// ✅ 2. Get Aiven PostgreSQL connection details
var host = Environment.GetEnvironmentVariable("AIVEN_HOST");
var port = Environment.GetEnvironmentVariable("AIVEN_PORT");
var database = Environment.GetEnvironmentVariable("AIVEN_DATABASE");
var username = Environment.GetEnvironmentVariable("AIVEN_USERNAME");
var password = Environment.GetEnvironmentVariable("AIVEN_PASSWORD");
var sslmode = Environment.GetEnvironmentVariable("AIVEN_SSLMODE");

if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(database) ||
    string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
{
    throw new Exception("Missing one or more required Aiven environment variables.");
}

var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode={sslmode}";

// ✅ 3. Register Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ✅ 4. Register custom services (dependency injection)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventParticipantService, EventParticipantService>();


// ✅ 5. Build & run the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();