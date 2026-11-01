using Microsoft.EntityFrameworkCore;
using MoviesCatalog.DAL.Data;
using MoviesCatalog.DAL.Entities;
using MoviesCatalog.DAL.Interfaces;

namespace MoviesCatalog.DAL.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _context;

    public MovieRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        return await _context.Movies
            .Include(m => m.Genres)
            .Include(m => m.Producer)
            .ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies
            .Include(m => m.Genres)
            .Include(m => m.Producer)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        var existing = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movie.Id);
        if (existing == null) return false;

        existing.Title = movie.Title;
        existing.Description = movie.Description;
        existing.ReleaseYear = movie.ReleaseYear;
        existing.Rating = movie.Rating;
        existing.ProducerId = movie.ProducerId;
        // Note to oneself: Updating Genres (Many-to-Many) is a bit more complex

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }
}
