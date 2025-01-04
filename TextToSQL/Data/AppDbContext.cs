using Bogus;
using Microsoft.EntityFrameworkCore;
using TextToSQL.Data.Models;

namespace TextToSQL.Data;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }   
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}