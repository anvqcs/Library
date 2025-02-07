using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;

namespace Library.UseCases.Features.Books.Commands.DeleteBook;

public record DeleteBookCommand(int id) : IRequest;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IGenericRepository<Book> _repository;
    public DeleteBookCommandHandler(IGenericRepository<Book> repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException($"{nameof(Book)} with ID {request.id} not found.");
        }
        await _repository.DeleteAsync(entity, cancellationToken);
    }
}