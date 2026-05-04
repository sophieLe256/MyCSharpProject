// Create a builder that configures services and the app's request pipeline
using WebApplication1.Endpoints;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation(); // Register the GameStoreContext with the dependency injection container
builder.AddGameStoreDb();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Build the configured application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGamesEndpoints();
// Handle GET requests to the root URL "/" and return "Hello World"
// app.MapGet("/", () => "Hello World");

app.MigrationDb(); // Ensure the database is migrated to the latest version on application startup

// Start the web server and begin listening for incoming HTTP requests
app.Run();
