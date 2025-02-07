using AutoMapper;
using Library.Core.Common.Mappings;
using Library.Core.Common.Models;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.UseCases.Features.Books.Queries.GetBooksWithPagination;

public record GetBooksWithPaginationQuery : IRequest<PaginatedList<BookBriefDto>>
{
    public string? Title { get; init; }
    public string? Author { get; init; }
    public int GenreId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetBooksWithPaginationQuery, PaginatedList<BookBriefDto>>
{
    private readonly ILibraryDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsWithPaginationQueryHandler(ILibraryDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BookBriefDto>> Handle(GetBooksWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Books!
            .Include(b => b.Genre)
            .Where(b => (string.IsNullOrEmpty(request.Title) || b.Title!.Contains(request.Title)) &&
                        (string.IsNullOrEmpty(request.Author) || b.Author!.Contains(request.Author)) &&
                        (request.GenreId == 0 || b.GenreId == request.GenreId))
            .OrderBy(b => b.Id)
            .Select(b => _mapper.Map<BookBriefDto>(b))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}