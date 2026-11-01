using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.DAL.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre> CreateAsync(Genre genre);
    Task<bool> UpdateAsync(Genre genre);
    Task<bool> DeleteAsync(int id);
}