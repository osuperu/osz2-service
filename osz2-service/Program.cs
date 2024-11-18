using osu.Game.Beatmaps.Formats;
using osu.Game.Rulesets;

var builder = WebApplication.CreateBuilder(args);

// Register rulesets
Decoder.RegisterDependencies(new AssemblyRulesetStore());

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();
