using MoviesCatalog.BLL.DTOs;

namespace MoviesCatalog.BLL.Interfaces;

public interface IProducerService
{
    Task<List<ProducerDto>> GetAllAsync();
    Task<ProducerDto?> GetByIdAsync(int id);
    Task<ProducerDto> CreateAsync(ProducerDto dto);
    Task<bool> UpdateAsync(int id, ProducerDto dto);
    Task<bool> DeleteAsync(int id);
}