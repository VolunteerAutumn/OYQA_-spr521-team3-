using MoviesCatalog.BLL;
using MoviesCatalog.DAL.Data;
using MoviesCatalog.Middlewares;
using MoviesCatalog.Presentation.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

FileLogger.ClearLog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

builder.Services.AddBll(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<TimestampMW>();
app.UseMiddleware<BasicInfoMW>();
app.UseMiddleware<IPMW>();
app.UseMiddleware<RequestCounterMW>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        DbSeeder.Seed(context, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error during database migration or crash.");
    }
}

app.Run();