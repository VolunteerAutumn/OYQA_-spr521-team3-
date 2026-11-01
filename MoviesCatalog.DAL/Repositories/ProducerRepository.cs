using Microsoft.EntityFrameworkCore;
using MoviesCatalog.DAL.Data; // Adjust if your namespace is different
using MoviesCatalog.DAL.Entities;
using MoviesCatalog.DAL.Interfaces;

namespace MoviesCatalog.DAL.Repositories;

public class ProducerRepository : IProducerRepository
{
    private readonly AppDbContext _context;

    public ProducerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producer>> GetAllAsync()
    {
        return await _context.Producers.ToListAsync();
    }

    public async Task<Producer?> GetByIdAsync(int id)
    {
        return await _context.Producers
            .Include(p => p.Movies) // Load the movies this producer made
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Producer> CreateAsync(Producer producer)
    {
        _context.Producers.Add(producer);
        await _context.SaveChangesAsync();
        return producer;
    }

    public async Task<bool> UpdateAsync(Producer producer)
    {
        var existing = await _context.Producers.FirstOrDefaultAsync(p => p.Id == producer.Id);
        if (existing == null) return false;

        existing.Name = producer.Name;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var producer = await _context.Producers.FirstOrDefaultAsync(p => p.Id == id);
        if (producer == null) return false;

        _context.Producers.Remove(producer);
        await _context.SaveChangesAsync();
        return true;
    }
}