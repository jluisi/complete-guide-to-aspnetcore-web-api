using System;
using System.Linq;
using System.Collections.Generic;

using my_books.Data.Models;
using my_books.Data.ViewModels;
using System.Text.RegularExpressions;
using my_books.Exceptions;

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
    public Publisher AddPublisher(PublisherVM publisher)
    {
      if (StringStartsWithNumber(publisher.Name))
      {
        throw new PublisherNameException("Name cannot start with a Number", publisher.Name);
      }

      var _publisher = new Publisher()
      {
        Name = publisher.Name
      };
      _context.Publishers.Add(_publisher);
      _context.SaveChanges();

      return _publisher;
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
      else
      {
        throw new Exception($"The Publisher with Id: {publisherId} does not exist");
      }
    }

    //---------------------------------------------------------------------------------------------
    private bool StringStartsWithNumber(string name)
    {
      return Regex.IsMatch(name, @"^\d");
    }
  }
}
