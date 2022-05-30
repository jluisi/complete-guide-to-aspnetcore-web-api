using System.Linq;
using System.Collections.Generic;

using my_books.Data.Models;
using my_books.Data.ViewModels;

namespace my_books.Data.Services
{
  public class PublishersService
  {
    private readonly AppDbContext _context;

    public PublishersService(AppDbContext context)
    {
      _context = context;
    }

    //---------------------------------------------------------------------------------------------
    public List<Publisher> GetAllPublishers()
    {
      return _context.Publishers.ToList();
    }

    //---------------------------------------------------------------------------------------------
    public Publisher GetPublisherById(int publisherId)
    {
      return _context.Publishers.FirstOrDefault(n => n.Id == publisherId);
    }

    //---------------------------------------------------------------------------------------------
    public PublisherWithBooksAuthorsVM GetPublisherData(int publisherId)
    {
      var _publisherData = _context.Publishers.Where(p => p.Id == publisherId)
        .Select(publisher => new PublisherWithBooksAuthorsVM()
        {
          Name = publisher.Name,
          BookAuthors = publisher.Books.Select(b => new BookAuthorVM()
          {
            BookName = b.Title,
            BookAuthors = b.Book_Authors.Select(ba => ba.Author.FullName).ToList()
          }).ToList()
        }).FirstOrDefault();

      return _publisherData;
    }

    //---------------------------------------------------------------------------------------------
    public void AddPublisher(PublisherVM publisher)
    {
      var _publisher = new Publisher()
      {
        Name = publisher.Name
      };
      _context.Publishers.Add(_publisher);
      _context.SaveChanges();
    }

    //---------------------------------------------------------------------------------------------
    public Publisher UpdatePublisherById(int publisherId, PublisherVM publisher)
    {
      var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == publisherId);

      if (_publisher != null)
      {
        _publisher.Name = publisher.Name;
        _context.SaveChanges();
      }
      return _publisher;
    }

    //---------------------------------------------------------------------------------------------
    public void DeletePublisherById(int publisherId)
    {
      var _publisher = _context.Publishers.FirstOrDefault(p => p.Id == publisherId);

      if (_publisher != null)
      {
        _context.Publishers.Remove(_publisher);
        _context.SaveChanges();
      }
    }
  }
}
