using AutoMapper;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.UseCases.Features.Books.Queries.GetBookById;

public class GetBookByIdQuery : IRequest<BookDto>
{
    public int Id { get; init; }
}

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
{
    private readonly ILibraryDbContext _context;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(ILibraryDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _context.Books!
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
        return _mapper.Map<BookDto>(book);
    }
}