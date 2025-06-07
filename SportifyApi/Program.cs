using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Services;
using SportifyApi.Interfaces;
using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.Extensions.FileProviders; 
using System.IO; 



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

if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(database) ||
    string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
{
    throw new Exception("Missing one or more required Aiven environment variables.");
}

var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode={sslmode}";


builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventParticipantService, EventParticipantService>();
builder.Services.AddScoped<IUserAchievementService, UserAchievementService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IOrganizationProfileService, OrganizationProfileService>();
builder.Services.AddScoped<IFriendService, FriendService>();







builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173", // Local dev
            "https://sportifydebuggers.netlify.app" // ✅ Netlify live frontend
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

// ✅ Seed Achievements (Only once at startup)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.SeedAchievements(dbContext);
}


app.UseCors("AllowViteFrontend");

// ✅ Always enable Swagger (even in production like Render)
app.UseSwagger();
app.UseSwaggerUI();


// ✅ Serve wwwroot (CSS, JS, etc.)
app.UseStaticFiles();

// ✅ Serve /uploads from wwwroot/uploads
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

// app.UseHttpsRedirection(); 


app.UseAuthorization();
app.MapControllers();
app.Run();
