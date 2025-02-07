using Library.Core.Constants;
using Library.UseCases.Features.ApplicationUsers.Commands.DeleteApplicationUser;
using Library.UseCases.Features.ApplicationUsers.Commands.UpdateApplicationUser;
using Library.UseCases.Features.ApplicationUsers.Queries.GetApplicationUserById;
using Library.UseCases.Features.ApplicationUsers.Queries.GetApplicationUsersWithPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers.Identity;

[Route("api/users")]
[ApiController]
public class ApplicationUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    [Authorize(Roles = nameof(Roles.Administrator))]
    public async Task<IActionResult> GetApplicationUsersWithPagination([FromQuery] GetApplicationUsersWithPaginationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetApplicationUserById(string id)
    {
        var query = new GetApplicationUserByIdQuery {id = id};
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateApplicationUser([FromBody] UpdateApplicationUserCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(Roles.Administrator))]
    public async Task<IActionResult> DeleteApplicationUser(string id)
    {
        var command = new DeleteApplicationUserCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}