using System.ComponentModel.DataAnnotations;

namespace MoviesCatalog.DAL.Entities;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }
    public int Rating { get; set; }

    // Relationship: A Movie has one Producer
    public int ProducerId { get; set; }
    public Producer Producer { get; set; } = null!;

    // Relationship: A Movie has many Genres
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}