using System.Security.Claims;
using AutoMapper;
using Library.Core.Common.Mappings;
using Library.Core.Common.Models;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.UseCases.Features.BorrowRecords.Queries.GetBorrowRecordsByUserWithPagination;

public record GetBorrowRecordsByUserWithPaginationQuery : IRequest<PaginatedList<BorrowRecordBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetBorrowRecordsByUserWithPaginationQueryHandler : IRequestHandler<GetBorrowRecordsByUserWithPaginationQuery, PaginatedList<BorrowRecordBriefDto>>
{
    private readonly ILibraryDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetBorrowRecordsByUserWithPaginationQueryHandler(ILibraryDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<PaginatedList<BorrowRecordBriefDto>> Handle(GetBorrowRecordsByUserWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _context.BorrowRecords!
            .Include(br => br.Book)
            .Where(br => br.ApplicationUserId == userId)
            .OrderByDescending(br => br.BorrowDate)
            .Select(br => _mapper.Map<BorrowRecordBriefDto>(br))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}