using AutoMapper;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;
using MoviesCatalog.DAL.Entities;
using MoviesCatalog.DAL.Interfaces;

namespace MoviesCatalog.BLL.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;

    public GenreService(IGenreRepository genreRepository, IMapper mapper)
    {
        _genreRepository = genreRepository;
        _mapper = mapper;
    }

    public async Task<List<GenreDto>> GetAllAsync()
    {
        var genres = await _genreRepository.GetAllAsync();
        return _mapper.Map<List<GenreDto>>(genres);
    }

    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        return genre == null ? null : _mapper.Map<GenreDto>(genre);
    }

    public async Task<GenreDto> CreateAsync(GenreDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);
        var createdGenre = await _genreRepository.CreateAsync(genre);
        return _mapper.Map<GenreDto>(createdGenre);
    }

    public async Task<bool> UpdateAsync(int id, GenreDto dto)
    {
        var genre = await _genreRepository.GetByIdAsync(id);

        if (genre == null)
            return false;

        genre.Name = dto.Name;

        return await _genreRepository.UpdateAsync(genre);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _genreRepository.DeleteAsync(id);
    }
}