using Microsoft.AspNetCore.Mvc;

using my_books.Data.Services;
using my_books.Data.ViewModels;

namespace my_authors.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthorsController : ControllerBase
  {
    private readonly AuthorsService _authorsService;

    public AuthorsController(AuthorsService authorsService)
    {
      _authorsService = authorsService;
    }

    //---------------------------------------------------------------------------------------------
    [HttpGet("get-all-authors")]
    public IActionResult GetAllAuthors()
    {
      var allAuthors = _authorsService.GetAllAuthors();
      return Ok(allAuthors);
    }

    //---------------------------------------------------------------------------------------------
    [HttpGet("get-author-by-id/{id}")]
    public IActionResult GetAuthorById(int id)
    {
      var author = _authorsService.GetAuthorById(id);
      return Ok(author);
    }

    //---------------------------------------------------------------------------------------------
    [HttpPost("add-author")]
    public IActionResult AddAuthor([FromBody] AuthorVM author)
    {
      _authorsService.AddAuthor(author);
      return Ok();
    }

    //---------------------------------------------------------------------------------------------
    [HttpPut("update-author-by-id/{id}")]
    public IActionResult UpdateAuthorById(int id, [FromBody] AuthorVM author)
    {
      var updatedAuthor = _authorsService.UpdateAuthorById(id, author);
      return Ok(updatedAuthor);
    }

    //---------------------------------------------------------------------------------------------
    [HttpDelete("delete-author-by-id/{id}")]
    public IActionResult DeleteAuthorById(int id)
    {
      _authorsService.DeleteAuthorById(id);
      return Ok();
    }
  }
}
