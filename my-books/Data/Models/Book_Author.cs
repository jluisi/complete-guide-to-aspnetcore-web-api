
namespace my_books.Data.Models
{
  public class Book_Author
  {
    public int Id { get; set; }

    // Navigation Properties - Foreign Keys to Book and Author
    public int BookId { get; set; }
    public Book Book { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }
  }
}
