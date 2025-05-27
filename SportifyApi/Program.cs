using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Services;
using SportifyApi.Interfaces;
using DotNetEnv;
using System.Formats.Tar;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load .env only during local development
#if DEBUG
DotNetEnv.Env.Load("../.env"); // Adjust path if needed
#endif

var host = Environment.GetEnvironmentVariable("AIVEN_HOST");
var port = Environment.GetEnvironmentVariable("AIVEN_PORT");
var database = Environment.GetEnvironmentVariable("AIVEN_DATABASE");
var username = Environment.GetEnvironmentVariable("AIVEN_USERNAME");
var password = Environment.GetEnvironmentVariable("AIVEN_PASSWORD");
var sslmode = Environment.GetEnvironmentVariable("AIVEN_SSLMODE");

if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(database) 
    || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
{
    throw new Exception("Missing one or more required Aiven environment variables.");
}

var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode={sslmode}";


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventParticipantService, EventParticipantService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors("AllowViteFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();
app.Run();