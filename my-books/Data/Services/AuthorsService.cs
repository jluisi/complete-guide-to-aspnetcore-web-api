using System.Linq;
using System.Collections.Generic;

using my_books.Data.Models;
using my_books.Data.ViewModels;

namespace my_books.Data.Services
{
  public class AuthorsService
  {
    private readonly AppDbContext _context;

    public AuthorsService(AppDbContext context)
    {
      _context = context;
    }

    //---------------------------------------------------------------------------------------------
    public List<Author> GetAllAuthors()
    {
      return _context.Authors.ToList();
    }

    //---------------------------------------------------------------------------------------------
    //public Author GetAuthorById(int authorId)
    //{
    //  return _context.Authors.FirstOrDefault(n => n.Id == authorId);
    //}
    
    //---------------------------------------------------------------------------------------------
    public AuthorWithBooksVM GetAuthorById(int authorId)
    {
      var _authorWithBooks = _context.Authors.Where(author => author.Id == authorId)
        .Select(author => new AuthorWithBooksVM()
        {
          FullName = author.FullName,
          BookIds = author.Book_Authors.Select(ba => ba.Book.Id).ToList(),
          BookTitles = author.Book_Authors.Select(ba => ba.Book.Title).ToList(),
        }).FirstOrDefault();

      return _authorWithBooks;
    }

    //---------------------------------------------------------------------------------------------
    public void AddAuthor(AuthorVM author)
    {
      var _author = new Author()
      {
        FullName = author.FullName
      };
      _context.Authors.Add(_author);
      _context.SaveChanges();
    }

    //---------------------------------------------------------------------------------------------
    public Author UpdateAuthorById(int authorId, AuthorVM author)
    {
      var _author = _context.Authors.FirstOrDefault(n => n.Id == authorId);

      if (_author != null)
      {
        _author.FullName = author.FullName;
        _context.SaveChanges();
      }
      return _author;
    }

    //---------------------------------------------------------------------------------------------
    public void DeleteAuthorById(int authorId)
    {
      var _author = _context.Authors.FirstOrDefault(n => n.Id == authorId);

      if (_author != null)
      {
        _context.Authors.Remove(_author);
        _context.SaveChanges();
      }
    }

  }
}
