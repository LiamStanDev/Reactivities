using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); // my middleware

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
        // service 為IServiceProvider: 為IOC容器的通用接口
        // GetRequireService() : 若沒有得到Service拋出異常
        // GetService() : 若沒有返回null
        var context = services.GetRequiredService<DataContext>(); // 從IOC容器中取得
        await context.Database.MigrateAsync();
        await Seed.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration.");
    }
}

app.Run();
