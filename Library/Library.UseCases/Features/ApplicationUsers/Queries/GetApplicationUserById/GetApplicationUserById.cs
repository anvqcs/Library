using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.UseCases.Features.ApplicationUsers.Queries.GetApplicationUserById;

public class GetApplicationUserByIdQuery : IRequest<ApplicationUser>
{
    public required string id { get; init; }
}

public class GetApplicationUserByIdQueryHandler : IRequestHandler<GetApplicationUserByIdQuery, ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetApplicationUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> Handle(GetApplicationUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.FindByIdAsync(request.id) ??
               throw new NotFoundException($"{nameof(ApplicationUser)} with ID {request.id} not found.");
    }
}