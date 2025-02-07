using Library.Core.Constants;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.UseCases.Features.Auths.Commands.SignUp;

public class SignUpCommand : IRequest<SignUpResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public SignUpCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Password and Confirm Password do not match");
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        var userExists = await _userManager.FindByNameAsync(request.Email);
        if (userExists != null) throw new ArgumentException("User already exists");
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));
        await SetDefaultRoleAsync(user);
        return new SignUpResponse
        {
            Token = await _jwtTokenGenerator.GenerateTokenAsync(user)
        };
    }

    private async Task SetDefaultRoleAsync(ApplicationUser user)
    {
        if (!await _roleManager.RoleExistsAsync(Roles.Reader))
        {
            var identityRole = new IdentityRole { Name = Roles.Reader };
            await _roleManager.CreateAsync(identityRole);
        }

        await _userManager.AddToRoleAsync(user, Roles.Reader);
    }
}

public class SignUpResponse
{
    public string? Token { get; set; }
}