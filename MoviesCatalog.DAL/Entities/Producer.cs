using System.ComponentModel.DataAnnotations;

namespace MoviesCatalog.DAL.Entities;

public class Producer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}