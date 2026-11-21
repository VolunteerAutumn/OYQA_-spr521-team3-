using AutoMapper;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;
using MoviesCatalog.DAL.Entities;
using MoviesCatalog.DAL.Interfaces;

namespace MoviesCatalog.BLL.Services;

public class ProducerService : IProducerService
{
    private readonly IProducerRepository _producerRepository;
    private readonly IMapper _mapper;

    public ProducerService(IProducerRepository producerRepository, IMapper mapper)
    {
        _producerRepository = producerRepository;
        _mapper = mapper;
    }

    public async Task<List<ProducerDto>> GetAllAsync()
    {
        var producers = await _producerRepository.GetAllAsync();
        return _mapper.Map<List<ProducerDto>>(producers);
    }

    public async Task<ProducerDto?> GetByIdAsync(int id)
    {
        var producer = await _producerRepository.GetByIdAsync(id);
        return producer == null ? null : _mapper.Map<ProducerDto>(producer);
    }

    public async Task<ProducerDto> CreateAsync(ProducerDto dto)
    {
        var producer = _mapper.Map<Producer>(dto);
        var createdProducer = await _producerRepository.CreateAsync(producer);
        return _mapper.Map<ProducerDto>(createdProducer);
    }

    public async Task<bool> UpdateAsync(int id, ProducerDto dto)
    {
        var producer = await _producerRepository.GetByIdAsync(id);

        if (producer == null)
            return false;

        producer.Name = dto.Name;

        return await _producerRepository.UpdateAsync(producer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _producerRepository.DeleteAsync(id);
    }
}