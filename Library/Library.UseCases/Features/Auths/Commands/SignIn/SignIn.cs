using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.UseCases.Features.Auths.Commands.SignIn;

public class SignInCommand : IRequest<SignInResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public SignInCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        var passwordValid = user != null && await _userManager.CheckPasswordAsync(user, request.Password);
        if (user == null || !passwordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        return new SignInResponse
        {
            Token = await _jwtTokenGenerator.GenerateTokenAsync(user)
        };
    }
}

public class SignInResponse
{
    public string? Token { get; set; }
}