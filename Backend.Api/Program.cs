using Microsoft.EntityFrameworkCore;
using Shared.Application.Services;
using Shared.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<SimulationEngine>();
builder.Services.AddScoped<DebriefService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", (policy) => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        Console.WriteLine("Testing DB connection...");
        var canConnect = db.Database.CanConnect();
        Console.WriteLine($"Can connect: {canConnect}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("DB connection failed:");
        Console.WriteLine(ex.ToString());
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();