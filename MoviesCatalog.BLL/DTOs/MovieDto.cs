
namespace MoviesCatalog.BLL.DTOs;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int Rating { get; set; }
    public ProducerDto? Producer { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
}
