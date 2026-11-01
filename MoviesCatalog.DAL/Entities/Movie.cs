using System.ComponentModel.DataAnnotations;

namespace MoviesCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Producer { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        
        public int Rating { get; set; }
    }
}
