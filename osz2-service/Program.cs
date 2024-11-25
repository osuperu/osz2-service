using osu.Game.Beatmaps.Formats;
using osu.Game.Rulesets;

var builder = WebApplication.CreateBuilder(args);

// Register rulesets
Decoder.RegisterDependencies(new AssemblyRulesetStore());

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

// Setup a ~100mb limit for file uploads
app.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600;
});
app.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 104857600;
});


app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();
