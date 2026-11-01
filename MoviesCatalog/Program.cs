using Microsoft.EntityFrameworkCore;
using MoviesCatalog.DAL;
using MoviesCatalog.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Add this

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MoviesCatalog.DAL.Interfaces.IMovieRepository, MoviesCatalog.DAL.Repositories.MovieRepository>();
builder.Services.AddScoped<MoviesCatalog.DAL.Interfaces.IGenreRepository, MoviesCatalog.DAL.Repositories.GenreRepository>();
builder.Services.AddScoped<MoviesCatalog.DAL.Interfaces.IProducerRepository, MoviesCatalog.DAL.Repositories.ProducerRepository>();

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