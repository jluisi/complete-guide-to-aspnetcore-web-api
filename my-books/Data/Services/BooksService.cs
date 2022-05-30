using System;
using System.Linq;
using System.Collections.Generic;

using my_books.Data.Models;
using my_books.Data.ViewModels;

namespace my_books.Data.Services
{
  public class BooksService
  {
    private readonly AppDbContext _context;

    public BooksService(AppDbContext context)
    {
      _context = context;
    }

    //---------------------------------------------------------------------------------------------
    public List<Book> GetAllBooks()
    {
      return _context.Books.ToList();
    }

    //---------------------------------------------------------------------------------------------
    //public Book GetBookById(int bookId)
    //{
    //  return _context.Books.FirstOrDefault(n => n.Id == bookId);
    //}

    //---------------------------------------------------------------------------------------------
    public BookWithAuthorsVM GetBookById(int bookId)
    {
      var _bookWithAuthors = _context.Books.Where(b => b.Id == bookId)
        .Select(book => new BookWithAuthorsVM()
        {
          Title = book.Title,
          Description = book.Description,
          IsRead = book.IsRead,
          DateRead = book.IsRead ? book.DateRead.Value : null,
          Rate = book.IsRead ? book.Rate.Value : null,
          Genre = book.Genre,
          CoverUrl = book.CoverUrl,
          PublisherId = book.PublisherId,
          PublisherName = book.Publisher.Name,
          AuthorIds = book.Book_Authors.Select(ba => ba.Author.Id).ToList(),
          AuthorNames = book.Book_Authors.Select(ba => ba.Author.FullName).ToList(),
        }).FirstOrDefault();

      return _bookWithAuthors;
    }

    //---------------------------------------------------------------------------------------------
    public void AddBookWithAuthors(BookVM book)
    {
      var _book = new Book()
      {
        Title = book.Title,
        Description = book.Description,
        IsRead = book.IsRead,
        DateRead = book.IsRead ? book.DateRead.Value : null,
        Rate = book.IsRead ? book.Rate.Value : null,
        Genre = book.Genre,
        CoverUrl = book.CoverUrl,
        DateAdded = DateTime.Now,
        PublisherId = book.PublisherId,
      };

      // After saving the changes, the _book object will have the Id of the new Book
      _context.Books.Add(_book);
      _context.SaveChanges();

      // Add the Author Ids to the Join Entity Book_Author and then to the Database
      foreach (var id in book.AuthorIds)
      {
        var _book_author = new Book_Author()
        {
          BookId = _book.Id,  // now the Id contains the value of the new Book !!!
          AuthorId = id,
        };

        // Similarly, the new Id of Book_Authors will be assigned to each instance
        _context.Books_Authors.Add(_book_author);
        _context.SaveChanges();
      }
    }

    //---------------------------------------------------------------------------------------------
    //public void AddBook(BookVM book)
    //{
    //  var _book = new Book()
    //  {
    //    Title = book.Title,
    //    Description = book.Description,
    //    IsRead = book.IsRead,
    //    DateRead = book.IsRead ? book.DateRead.Value : null,
    //    Rate = book.IsRead ? book.Rate.Value : null,
    //    Genre = book.Genre,
    //    Author = book.Author,
    //    CoverUrl = book.CoverUrl,
    //    DateAdded = DateTime.Now,
    //  };
    //  _context.Books.Add(_book);
    //  _context.SaveChanges();
    //}

    //---------------------------------------------------------------------------------------------
    public Book UpdateBookById(int bookId, BookVM book)
    {
      var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);

      if (_book != null)
      {
        _book.Title = book.Title;
        _book.Description = book.Description;
        _book.IsRead = book.IsRead;
        _book.DateRead = book.IsRead ? book.DateRead.Value : null;
        _book.Rate = book.IsRead ? book.Rate.Value : null;
        _book.Genre = book.Genre;
        _book.CoverUrl = book.CoverUrl;
        _context.SaveChanges();
      }
      return _book;
    }

    //---------------------------------------------------------------------------------------------
    public void DeleteBookById(int bookId)
    {
      var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);

      if (_book != null)
      {
        _context.Books.Remove(_book);
        _context.SaveChanges();
      }
    }

  }
}
