using Microsoft.EntityFrameworkCore;

using my_books.Data.Models;

namespace my_books.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Foreing Key to the Books Table
      modelBuilder.Entity<Book_Author>()
        .HasOne(b => b.Book)
        .WithMany(ba => ba.Book_Authors)
        .HasForeignKey(ba => ba.BookId);

      // Foreing Key to the Authors Table
      modelBuilder.Entity<Book_Author>()
        .HasOne(b => b.Author)
        .WithMany(ba => ba.Book_Authors)
        .HasForeignKey(ba => ba.AuthorId);
    }

    //---------------------------------------------------------------------------------------------
    // DbSets
    //---------------------------------------------------------------------------------------------
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book_Author> Books_Authors { get; set; }
    public DbSet<Publisher> Publishers { get; set; }

  }
}
