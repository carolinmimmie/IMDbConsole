using System.ComponentModel.DataAnnotations;

namespace IMDb.Domain;

class Actor
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }

    // Navigation property
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}