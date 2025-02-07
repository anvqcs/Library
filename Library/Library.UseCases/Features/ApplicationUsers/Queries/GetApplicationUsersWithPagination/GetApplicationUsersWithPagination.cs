using Library.Core.Common.Mappings;
using Library.Core.Common.Models;
using Library.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.UseCases.Features.ApplicationUsers.Queries.GetApplicationUsersWithPagination;

public class GetApplicationUsersWithPaginationQuery : IRequest<PaginatedList<ApplicationUser>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public class GetApplicationUsersWithPaginationQueryHandler : IRequestHandler<GetApplicationUsersWithPaginationQuery,
    PaginatedList<ApplicationUser>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetApplicationUsersWithPaginationQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PaginatedList<ApplicationUser>> Handle(GetApplicationUsersWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        return await _userManager.Users.PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}