using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;

namespace Library.UseCases.Features.Genres.Queries.GetGenreById;

public record GetGenreByIdQuery (int id) : IRequest<Genre>;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Genre>
{
    private readonly IGenericRepository<Genre> _repository;
    public GetGenreByIdQueryHandler(IGenericRepository<Genre> repository)
    {
        _repository = repository;
    }
    public async Task<Genre> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.id, cancellationToken) ??
               throw new NotFoundException($"{nameof(ApplicationUser)} with ID {request.id} not found.");
    }
}
