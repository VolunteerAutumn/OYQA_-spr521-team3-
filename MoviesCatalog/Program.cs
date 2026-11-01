using Microsoft.EntityFrameworkCore;
using MoviesCatalog.DAL;
using MoviesCatalog.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Add this

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Add this
    app.UseSwaggerUI(); // Add this
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();