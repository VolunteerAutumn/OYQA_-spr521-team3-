using Microsoft.AspNetCore.Mvc;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;

namespace MoviesCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProducersController : ControllerBase
{
    private readonly IProducerService _producerService;

    public ProducersController(IProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProducerDto>>> GetAll()
    {
        return Ok(await _producerService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProducerDto>> GetById(int id)
    {
        var producer = await _producerService.GetByIdAsync(id);
        if (producer == null) return NotFound(new { message = "Producer not found." });
        return Ok(producer);
    }

    [HttpPost]
    public async Task<ActionResult<ProducerDto>> Create(ProducerDto dto)
    {
        var created = await _producerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProducerDto dto)
    {
        var updated = await _producerService.UpdateAsync(id, dto);
        if (!updated) return NotFound(new { message = "Producer not found." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _producerService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "Producer not found." });
        return NoContent();
    }
}