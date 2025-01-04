namespace TextToSQL.Data.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = new List<Book>();
}