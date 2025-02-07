using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;

namespace Library.UseCases.Features.BorrowRecords.Commands.DeleteBorrowRecord;

public record DeleteBorrowRecordCommand(int id) : IRequest;

public class DeleteBorrowRecordCommandHandler : IRequestHandler<DeleteBorrowRecordCommand>
{
    private readonly IGenericRepository<BorrowRecord> _repository;
    public DeleteBorrowRecordCommandHandler(IGenericRepository<BorrowRecord> repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteBorrowRecordCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException($"{nameof(BorrowRecord)} with ID {request.id} not found.");
        }
        await _repository.DeleteAsync(entity, cancellationToken);
    }
}