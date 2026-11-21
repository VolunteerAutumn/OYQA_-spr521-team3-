using AutoMapper;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MovieDto>().ReverseMap();

        CreateMap<MovieCreateDto, Movie>()
            .ForMember(dest => dest.Genres, opt => opt.Ignore());

        CreateMap<Genre, GenreDto>().ReverseMap();
        CreateMap<Producer, ProducerDto>().ReverseMap();
    }
}