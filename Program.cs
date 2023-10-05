using System.Text.Json;
using IMDb.Domain;
using static System.Console;

namespace IMDb;

class Program
{

    // Httpclient finns tillgänglig, behöver inte installeras via NuGet
    //Den gör HTTP anrop över nätverket
    //För att göra HHTP.anrop ( text GET) behöver vi använda ett bibliotek
    //som kan göra detta. såsom HttpClient. Använder detta i våran metod GetForCast()

    static readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:8000/")//adressen
    };


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
        // 1 - Hämta movies från backend (web api) (alltså skicka en HHTP GET förfrågan)
        var movies = GetMovies();

        // 2 - Skriv ut en "tabell", på samma sätt som vi precis gjorde i vår web-applikation
        Write($"{"Titel",-16}");
        Write($"{"Genre",-16}");
        WriteLine("Director");

        foreach (var movie in movies)
        {
            Write($"{movie.Title,-16}");
            Write($"{movie.Genre,-16}");
            WriteLine(movie.Director);
        }

        // 3 - Vänta på att användaren trycker på escape, återvänd då till huvudmenyn
        WaitUntilKeyPressed(ConsoleKey.Escape);
    }



    // Vi vill Returnera IEnumerable <Movie>
    // IEnumarable inte är en datatyp i sig själv utan snarare
    // ett gränssnitt som används för att hantera upprepning över samlingar av data.

    private static IEnumerable<Movie> GetMovies()
    {
        // TODO Hämta samtliga filmer genom att skicka ett HTTP GET till https://localhost:8000/movies

        // 1 - Skicka ett HTTP GET-anrop till backend (web api)
        var response = httpClient.GetAsync("movies").Result;

        // 2 - Läs ut JSON som vi fått tillbaka
        var json = response.Content.ReadAsStringAsync().Result;

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // 3 - Deserialisera JSON till ett objekt (IEnumerable<WeatherForecast>)
        var movies = JsonSerializer
            .Deserialize<IEnumerable<Movie>>(json, serializeOptions)
            ?? new List<Movie>();

        // 4 - Returnera resultatet (IEnumerable<Movie>)
        return movies;

        throw new NotImplementedException();
    }

    private static void AddMovieView()
    {
        var title = GetUserInput("Titel");
        var plot = GetUserInput("Handling");
        var director = GetUserInput("Regissör");
        var genre = GetUserInput("Genre");
        var releaseYear = DateTime.Parse(GetUserInput("Premiär (YYYY-MM-DD)"));

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