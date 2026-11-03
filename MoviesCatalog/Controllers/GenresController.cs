using Microsoft.AspNetCore.Mvc;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;

namespace MoviesCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GenreDto>>> GetAll()
    {
        return Ok(await _genreService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetById(int id)
    {
        var genre = await _genreService.GetByIdAsync(id);
        if (genre == null) return NotFound(new { message = "Genre not found." });
        return Ok(genre);
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Create(GenreDto dto)
    {
        var created = await _genreService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GenreDto dto)
    {
        var updated = await _genreService.UpdateAsync(id, dto);
        if (!updated) return NotFound(new { message = "Genre not found." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _genreService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "Genre not found." });
        return NoContent();
    }
}