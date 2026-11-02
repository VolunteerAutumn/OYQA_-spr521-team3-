using MoviesCatalog.BLL.DTOs;

namespace MoviesCatalog.BLL.Interfaces;

public interface IGenreService
{
    Task<List<GenreDto>> GetAllAsync();
    Task<GenreDto?> GetByIdAsync(int id);
    Task<GenreDto> CreateAsync(GenreDto dto);
    Task<bool> UpdateAsync(int id, GenreDto dto);
    Task<bool> DeleteAsync(int id);
}