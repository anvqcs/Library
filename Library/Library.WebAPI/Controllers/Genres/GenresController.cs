using Library.UseCases.Features.Genres.Queries.GetGenreById;
using Library.UseCases.Features.Genres.Queries.GetGenres;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers.Genres;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var query = new GetGenresQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(int id)
    {
        var query = new GetGenreByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}