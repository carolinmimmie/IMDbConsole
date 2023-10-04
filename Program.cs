using IMDb.Domain;
using static System.Console;

namespace IMDb;

class Program
{
  public static void Main()
  {
    Title = "IMDb";

    while (true)
    {
      CursorVisible = false;

      WriteLine("1. Lägg till film");
      WriteLine("2. Lista filmer");

      var keyPressed = ReadKey(true);

      Clear();

      switch (keyPressed.Key)
      {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:

          AddMovieView();

          break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:

          ListMoviesView();

          break;
      }

      Clear();
    }
  }

  private static void ListMoviesView()
  {
    var movies = GetMovies();

    Write($"{"Titel",-16}");
    Write($"{"Genre",-16}");
    WriteLine("Director");

    foreach (var movie in movies)
    {
      Write($"{movie.Title,-16}");
      Write($"{movie.Genre,-16}");
      WriteLine(movie.Director);
    }

    WaitUntilKeyPressed(ConsoleKey.Escape);
  }

  private static IEnumerable<Movie> GetMovies()
  {
    // TODO Hämta samtliga filmer genom att skicka ett HTTP GET till https://localhost:8000/movies

    // Ta bort
    throw new NotImplementedException();
  }

  private static void AddMovieView()
  {
    var title = GetUserInput("Titel");
    var plot = GetUserInput("Handling");
    var director = GetUserInput("Regissör");
    var genre = GetUserInput("Genre");
    var releaseDate = DateTime.Parse(GetUserInput("Premiär (YYYY-MM-DD)"));

    var movie = new Movie
    {
      Title = title,
      Plot = plot,
      Director = director,
      Genre = genre,
      ReleaseDate = releaseDate
    };

    Clear();

    try
    {
      SaveMovie(movie);

      WriteLine("Film sparad");
    }
    catch
    {
      WriteLine("Ogiltig information");
    }

    Thread.Sleep(2000);
  }

  private static void SaveMovie(Movie movie)
  {
    // TODO Skicka information om filmen till web API:et genom att skicka 
    //      ett HTTP POST-anrop till https://localhost:8000/movies

    // Om statuskod är "400 Bad Request", kasta exception

    // Ta bort
    throw new NotImplementedException();
  }

  private static string GetUserInput(string label)
  {
    CursorVisible = true;

    Write($"{label}: ");

    return ReadLine() ?? "";
  }

  private static void WaitUntilKeyPressed(ConsoleKey key)
  {
    while (ReadKey(true).Key != key) ;
  }
}