
using AutoMapper;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<Genre, GenreDto>();
        CreateMap<Producer, ProducerDto>();
    }
}
