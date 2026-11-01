using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.DAL.Interfaces;

public interface IProducerRepository
{
    Task<List<Producer>> GetAllAsync();
    Task<Producer?> GetByIdAsync(int id);
    Task<Producer> CreateAsync(Producer producer);
    Task<bool> UpdateAsync(Producer producer);
    Task<bool> DeleteAsync(int id);
}