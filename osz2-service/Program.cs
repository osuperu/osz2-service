var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();

