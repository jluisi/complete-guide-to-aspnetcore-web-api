using System;
using Microsoft.AspNetCore.Mvc;
using my_books.ActionResults;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;

namespace my_publishers.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PublishersController : ControllerBase
  {
    private readonly PublishersService _publishersService;

    public PublishersController(PublishersService publishersService)
    {
      _publishersService = publishersService;
    }

    //---------------------------------------------------------------------------------------------
    [HttpGet("get-all-publishers")]
    public IActionResult GetAllPublishers(string sortBy, string filterBy, int pageNumber)
    {
      try
      {
        var allPublishers = _publishersService.GetAllPublishers(sortBy, filterBy, pageNumber);
        return Ok(allPublishers);
      }
      catch (Exception ex)
      {
        return BadRequest($"Publisher could not be loaded: {ex}");
      }
    }

    //---------------------------------------------------------------------------------------------
    [HttpGet("get-publisher-by-id/{id}")]
    public IActionResult GetPublisherById(int id)
    {
      var publisher = _publishersService.GetPublisherById(id);

      if (publisher != null)
      {
        return Ok(publisher);
      }
      else
      {
        return NotFound();
      }
    }

    //---------------------------------------------------------------------------------------------
    // Using a Custom Action Result Type
    //---------------------------------------------------------------------------------------------
    /*
    [HttpGet("get-publisher-by-id/{id}")]
    public CustomActionResult GetPublisherById(int id)
    {
      var publisher = _publishersService.GetPublisherById(id);

      if (publisher != null)
      {
        // Using a Custom Action Result
        var _responseObj = new CustomActionResultVM()
        {
          Publisher = publisher
        };

        return new CustomActionResult(_responseObj);
      }
      else
      {
        // Using a Custom Action Result
        var _responseObj = new CustomActionResultVM()
        {
          Exception = new Exception("Custom Exception from GetPublisherById Controller")
        };

        return new CustomActionResult(_responseObj);
      }
    }
    */

    //---------------------------------------------------------------------------------------------
    [HttpGet("get-publisher-books-with-authors/{id}")]
    public IActionResult GetPublisherData(int id)
    {
      var publisher = _publishersService.GetPublisherData(id);
      return Ok(publisher);
    }

    //---------------------------------------------------------------------------------------------
    [HttpPost("add-publisher")]
    public IActionResult AddPublisher([FromBody] PublisherVM publisher)
    {
      try
      {
        var newPublisher = _publishersService.AddPublisher(publisher);
        return Created(nameof(AddPublisher), newPublisher);
      }
      catch (PublisherNameException ex)
      {
        return BadRequest($"{ex.Message}, Publisher Name: {ex.PublisherName}");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //---------------------------------------------------------------------------------------------
    [HttpPut("update-publisher-by-id/{id}")]
    public IActionResult UpdatePublisherById(int id, [FromBody] PublisherVM publisher)
    {
      var updatedPublisher = _publishersService.UpdatePublisherById(id, publisher);
      return Ok(updatedPublisher);
    }

    //---------------------------------------------------------------------------------------------
    [HttpDelete("delete-publisher-by-id/{id}")]
    public IActionResult DeletePublisherById(int id)
    {
      try
      {
        _publishersService.DeletePublisherById(id);
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
