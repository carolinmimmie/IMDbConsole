using System.Text;
using System.Text.Json;
using IMDb.Domain;
using static System.Console;

namespace IMDb;

class Program
{
  static readonly HttpClient httpClient = new()
  {
    BaseAddress = new Uri("https://localhost:8000/")
  };

  public static void Main()
  {
    Title = "IMDb";

    while (true)
    {
      CursorVisible = false;

      WriteLine("1. Lägg till film");
      WriteLine("2. Lista filmer");
      WriteLine("3. Radera film");

      var keyPressed = ReadKey(true);

      Clear();

      switch (keyPressed.Key)
      {
        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:

          ListMoviesView();

          break;
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:

          AddMovieView();

          break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:

          DeleteMovieView();

          break;
      }

      Clear();
    }
  }

  private static void DeleteMovieView()
  {
    Write("Ange titel: ");

    var title = ReadLine();

    Clear();

    var movies = GetMovies(title); // skicka in title i GetMovies metoden

    foreach (var movie in movies)
    {
      WriteLine($"{movie.Id} - {movie.Title} ({movie.ReleaseYear})");
    }

    WriteLine();

    Write("Ange ID> ");

    var movieId = int.Parse(ReadLine());

    Clear();

    try
    {
      DeleteMovie(movieId);

      WriteLine("Film raderad");
    }
    catch
    {
      WriteLine("Film saknas");
    }

    Thread.Sleep(2000);
  }

  private static void DeleteMovie(int movieId)
  {
    // DELETE https://localhost:8000/movies/{id}
    var response = httpClient.DeleteAsync($"movies/{movieId}").Result;

    // Kommer kasta Exception om statuskod inte var något i 2xx-omfånget (t.ex. 404 Not Found)
    response.EnsureSuccessStatusCode();
  }

  private static void ListMoviesView()
  {
    var movies = GetMovies();

    Write($"{"Titel",-26}");
    Write($"{"Genre",-26}");
    WriteLine("Director");

    foreach (var movie in movies)
    {
      Write($"{movie.Title,-26}");
      Write($"{movie.Genre,-26}");
      WriteLine(movie.Director);
    }

    WaitUntilKeyPressed(ConsoleKey.Escape);
  }

  private static IEnumerable<Movie> GetMovies(string title = null) //strint title = null tillåter oss att anropa metoden med eller utan title
  {
    // TODO HTTP GET till https://localhost:8000/movies

    HttpResponseMessage response;
    if ( title is null ) 
    {

      // GET https://localhost:8000/movies
       response = httpClient.GetAsync("movies").
       Result; // om titel är null gör detta
    }
    else {

      // GET https://localhost:8000/movies?title={title}
      response = httpClient.GetAsync($"movies?title={title}")
      .Result; // 
    }

    var json = response.Content
        .ReadAsStringAsync()
        .Result;

    var serializeOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // Deserialisera från JSON till IEnumerable<Movie>
    var movies = JsonSerializer
        .Deserialize<IEnumerable<Movie>>(json, serializeOptions)
        ?? new List<Movie>();

    return movies;
  }

  private static void AddMovieView()
  {
    var title = GetUserInput("Titel");
    var plot = GetUserInput("Handling");
    var director = GetUserInput("Regissör");
    var genre = GetUserInput("Genre");
    var releaseYear = int.Parse(GetUserInput("År"));

    var movie = new Movie
    {
      Title = title,
      Plot = plot,
      Director = director,
      Genre = genre,
      ReleaseYear = releaseYear
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

    // 1 - Serialisera movie-objekt till JSON ({ "title": "Aliens", "plot": "Lorem ipsum dolor", ... })
    var json = JsonSerializer.Serialize(movie);

    var body = new StringContent(
      json,
      Encoding.UTF8,
      // Beskriver formatet på data
      "application/json");

    // POST https://localhost:8000/movies
    var response = httpClient.PostAsync("movies", body).Result;

    // Om statuskod är "400 Bad Request", kasta exception som du sedan fångar
    // där SaveMovie() anropas, och då visar meddelandet "Ogiltig information" i 2 sekunder.

    // Kasta exception om statuskoden inte ligger inom 2xx-omfånget.
    response.EnsureSuccessStatusCode();
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