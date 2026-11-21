using MoviesCatalog.BLL.DTOs;

namespace MoviesCatalog.BLL.Interfaces;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllAsync();
    Task<MovieDto?> GetByIdAsync(int id);
    Task<MovieDto> CreateAsync(MovieCreateDto dto);
    Task<bool> UpdateAsync(int id, MovieCreateDto dto);
    Task<bool> DeleteAsync(int id);
}