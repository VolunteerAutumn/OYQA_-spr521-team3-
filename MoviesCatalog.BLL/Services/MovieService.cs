using AutoMapper;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;
using MoviesCatalog.DAL.Entities;
using MoviesCatalog.DAL.Interfaces;

namespace MoviesCatalog.BLL.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IProducerRepository _producerRepository;
    private readonly IMapper _mapper;

    public MovieService(
        IMovieRepository movieRepository,
        IGenreRepository genreRepository,
        IProducerRepository producerRepository,
        IMapper mapper)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _producerRepository = producerRepository;
        _mapper = mapper;
    }

    public async Task<List<MovieDto>> GetAllAsync()
    {
        var movies = await _movieRepository.GetAllAsync();
        return _mapper.Map<List<MovieDto>>(movies);
    }

    public async Task<MovieDto?> GetByIdAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        return movie == null ? null : _mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDto> CreateAsync(MovieCreateDto dto)
    {
        var producer = await _producerRepository.GetByIdAsync(dto.ProducerId);

        if (producer == null)
            throw new ArgumentException("Producer not found.");

        var genres = new List<Genre>();

        foreach (var genreId in dto.GenreIds.Distinct())
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);

            if (genre == null)
                throw new ArgumentException($"Genre with ID {genreId} not found.");

            genres.Add(genre);
        }

        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            ReleaseYear = dto.ReleaseYear,
            Rating = dto.Rating,
            ProducerId = producer.Id,
            Producer = producer,
            Genres = genres
        };

        var createdMovie = await _movieRepository.CreateAsync(movie);

        return _mapper.Map<MovieDto>(createdMovie);
    }

    public async Task<bool> UpdateAsync(int id, MovieCreateDto dto)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null) return false;

        var producer = await _producerRepository.GetByIdAsync(dto.ProducerId);
        if (producer == null) throw new ArgumentException("Producer not found.");

        var genres = await _genreRepository.GetByIdsAsync(dto.GenreIds.Distinct().ToList());

        if (genres.Count != dto.GenreIds.Distinct().Count())
            throw new ArgumentException("One or more genres not found.");

        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.ReleaseYear = dto.ReleaseYear;
        movie.Rating = dto.Rating;
        movie.ProducerId = producer.Id;
        movie.Producer = producer;

        movie.Genres.Clear();
        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return await _movieRepository.UpdateAsync(movie);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _movieRepository.DeleteAsync(id);
    }
}