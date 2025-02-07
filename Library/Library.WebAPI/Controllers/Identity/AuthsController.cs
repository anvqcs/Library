using Library.UseCases.Features.Auths.Commands.SignIn;
using Library.UseCases.Features.Auths.Commands.SignUp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers.Identity;

[Route("api/auth")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] SignUpCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}