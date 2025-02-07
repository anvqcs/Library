using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.UseCases.Features.ApplicationUsers.Commands.DeleteApplicationUser;

public record DeleteApplicationUserCommand(string id) : IRequest;

public class DeleteApplicationUserCommandHandler : IRequestHandler<DeleteApplicationUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteApplicationUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.id);
        if (user is null) throw new NotFoundException($"{nameof(ApplicationUser)} with ID {request.id} not found.");
        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);
    }
}