using Microsoft.EntityFrameworkCore;
using UserProfileWebAPI.Data;
using UserProfileWebAPI.Controllers;
using UserProfileWebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registering ApplicationDbContext as a service with the IoC container.
// UseNpgsql specifies that the ApplicationDbContext should use PostgreSQL as the database provider.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers as a service. 
// This enables the IoC container to inject them wherever needed (like in routing and middleware).
builder.Services.AddControllers();

// Add Swagger for API documentation. The IoC container handles lifecycle management of Swagger services.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

var app = builder.Build();

// there are 2 ways to seend DB context.. 1st through DbContext file and other through DbInitializer. Both approaches have been implemented here.
// Seed the database with initial data via DbInitilizer if the DBcontext is not set
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
    }
}

// If in development mode, configure Swagger middleware to inject it into the app.
// The DI container ensures the necessary services are available here.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware for redirecting HTTP requests to HTTPS. DI is used to inject middleware dependencies.
app.UseHttpsRedirection();

// Add routing middleware
//app.UseRouting();

// Middleware for handling authorization. DI resolves authorization policies and handlers here.
//app.UseAuthorization();

// Map controllers to endpoints. DI injects controllers at runtime.
app.MapControllers();



// Run the application. Dependencies injected so far work seamlessly.
app.Run();
