using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.DAL.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task<Movie> CreateAsync(Movie movie);
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteAsync(int id);
}