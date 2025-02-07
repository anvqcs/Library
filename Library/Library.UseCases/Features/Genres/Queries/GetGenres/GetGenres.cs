using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;

namespace Library.UseCases.Features.Genres.Queries.GetGenres;

public class GetGenresQuery : IRequest<IEnumerable<Genre>>
{
}

public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IEnumerable<Genre>>
{
    private readonly IGenericRepository<Genre> _repository;

    public GetGenresQueryHandler(IGenericRepository<Genre> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        return await _repository.ListAsync(cancellationToken);
    }
}
