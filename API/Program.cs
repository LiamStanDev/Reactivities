using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt => 
    {
        opt.AddPolicy("CorsPolicy", policy =>
        {
            // policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
        });
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configure = builder.Configuration;
builder.Services.AddDbContext<DataContext>(opt => 
{
    opt.UseMySql(configure.GetConnectionString("Default"), ServerVersion.Parse("10.9.5-mariadb"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(); // need to be the first, because the browser gonna seed the cors request first.

app.UseAuthorization();

app.MapControllers();

// 建立Scope
// 注意：builder.Build()已經結束
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider; // 使用ServiceProvider 來使用IOC容器

    try
    {
        var context = services.GetRequiredService<DataContext>(); // 從IOC容器中取得
        await context.Database.MigrateAsync();
        await Seed.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration.");
    }
}

app.Run();
