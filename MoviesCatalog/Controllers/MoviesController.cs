using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;

namespace MoviesCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> GetAll()
    {
        return Ok(await _movieService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        if (movie == null) return NotFound(new { message = "Movie not found." });
        return Ok(movie);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create(MovieCreateDto dto)
    {
        try
        {
            var created = await _movieService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, MovieCreateDto dto)
    {
        try
        {
            var updated = await _movieService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { message = "Movie not found." });
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _movieService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "Movie not found." });
        return NoContent();
    }
}