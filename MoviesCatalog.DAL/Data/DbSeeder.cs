using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.DAL.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Movies.Any())
        {
            return;
        }

        if (!context.Producers.Any())
        {
            var producers = new List<Producer>
            {
                new Producer { Name = "Christopher Nolan" },
                new Producer { Name = "Frank Darabont" },
                new Producer { Name = "Peter Jackson" },
                new Producer { Name = "Guy Ritchie" }
            };
            context.Producers.AddRange(producers);
            context.SaveChanges();
        }

        if (!context.Genres.Any())
        {
            var genres = new List<Genre>
            {
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Fantasy" },
                new Genre { Name = "Crime" }
            };
            context.Genres.AddRange(genres);
            context.SaveChanges();
        }

        var nolan = context.Producers.First(p => p.Name == "Christopher Nolan");
        var darabont = context.Producers.First(p => p.Name == "Frank Darabont");
        var jackson = context.Producers.First(p => p.Name == "Peter Jackson");
        var ritchie = context.Producers.First(p => p.Name == "Guy Ritchie");

        var sciFi = context.Genres.First(g => g.Name == "Sci-Fi");
        var drama = context.Genres.First(g => g.Name == "Drama");
        var fantasy = context.Genres.First(g => g.Name == "Fantasy");
        var crime = context.Genres.First(g => g.Name == "Crime");

        var movies = new List<Movie>
        {
            new Movie 
            { 
                Title = "Interstellar", 
                Description = "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft...", 
                ReleaseYear = 2014, 
                Rating = 8, 
                ProducerId = nolan.Id,
                Genres = new List<Genre> { sciFi, drama }
            },
            new Movie 
            { 
                Title = "The Shawshank Redemption", 
                Description = "A banker convicted of uxoricide is sentenced to life imprisonment at the Shawshank prison...", 
                ReleaseYear = 1994, 
                Rating = 8, 
                ProducerId = darabont.Id,
                Genres = new List<Genre> { drama }
            },
            new Movie 
            { 
                Title = "The Green Mile", 
                Description = "A tale on death row set during the Great Depression in the American South...", 
                ReleaseYear = 1999, 
                Rating = 8, 
                ProducerId = darabont.Id,
                Genres = new List<Genre> { drama }
            },
            new Movie 
            { 
                Title = "The Lord of the Rings: The Return of the King", 
                Description = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam...", 
                ReleaseYear = 2003, 
                Rating = 8, 
                ProducerId = jackson.Id,
                Genres = new List<Genre> { fantasy, drama }
            },
            new Movie 
            { 
                Title = "The Gentlemen", 
                Description = "An American expat tries to sell out of his highly profitable marijuana empire in London...", 
                ReleaseYear = 2019, 
                Rating = 8, 
                ProducerId = ritchie.Id,
                Genres = new List<Genre> { crime }
            }
        };

        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}