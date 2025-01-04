namespace TextToSQL.Data.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public float Rating { get; set; }
    public int Pages { get; set; }
    public DateTime Published { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}