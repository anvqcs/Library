using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.UseCases.Features.ApplicationUsers.Commands.UpdateApplicationUser;

public record UpdateApplicationUserCommand : IRequest
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
}

public class UpdateApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateApplicationUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user is null) throw new NotFoundException($"{nameof(ApplicationUser)} with ID {request.Id} not found.");
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;
        await _userManager.UpdateAsync(user);
    }
}