using System.ComponentModel.DataAnnotations;

namespace MoviesCatalog.DAL.Entities;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    // Many-to-Many relationship back to Movies
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}