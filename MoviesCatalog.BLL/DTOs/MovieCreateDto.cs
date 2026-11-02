
namespace MoviesCatalog.BLL.DTOs;

public class MovieCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int Rating { get; set; }
    public int ProducerId { get; set; }
    public List<int> GenreIds { get; set; } = new();
}
