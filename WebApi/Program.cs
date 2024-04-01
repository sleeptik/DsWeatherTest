using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<WeatherArchiveContext>((provider, optionsBuilder) =>
{
    var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("Default");
    optionsBuilder.UseNpgsql(connectionString);
});

builder.Services.AddTransient<ArchiveService>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = serviceScopeFactory.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WeatherArchiveContext>();

    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}

app.Run();