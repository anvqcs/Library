using Library.UseCases.Features.Books.Commands.CreateBook;
using Library.UseCases.Features.Books.Commands.DeleteBook;
using Library.UseCases.Features.Books.Commands.UpdateBook;
using Library.UseCases.Features.Books.Queries.GetBookById;
using Library.UseCases.Features.Books.Queries.GetBooksWithPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers.Books;

[Route("api/books")]
[ApiController]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] GetBooksWithPaginationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var query = new GetBookByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var command = new DeleteBookCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] UpdateBookCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}