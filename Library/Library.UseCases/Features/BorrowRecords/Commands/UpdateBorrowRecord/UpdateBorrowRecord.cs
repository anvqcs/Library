using System.Security.Claims;
using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Library.UseCases.Features.BorrowRecords.Commands.UpdateBorrowRecord;

public record UpdateBorrowRecordCommand : IRequest
{
    public int Id { get; set; }
    public required int BookId { get; set; }
    public required string ApplicationUserId { get; set; }
    public double RentalCost { get; set; }
    public bool IsReturned { get; set; } = false;
}

public class UpdateBorrowRecordCommandHandler : IRequestHandler<UpdateBorrowRecordCommand>
{
    private readonly ILibraryDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<BorrowRecord> _repository;

    public UpdateBorrowRecordCommandHandler(IGenericRepository<BorrowRecord> repository, ILibraryDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpdateBorrowRecordCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException($"{nameof(BorrowRecord)} with ID {request.Id} not found.");
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        entity.BookId = request.BookId;
        entity.ApplicationUserId = request.ApplicationUserId;
        entity.RentalCost = request.RentalCost;
        entity.ModifiedBy = userId;
        entity.Modified = DateTime.Now;
        if (request.IsReturned)
        {
            entity.ReturnDate = DateTime.Now;
            await UpdateBookQuantity(request.BookId, cancellationToken);
        }

        await _repository.UpdateAsync(entity, cancellationToken);
    }

    private async Task UpdateBookQuantity(int bookId, CancellationToken cancellationToken)
    {
        var book = await _context.Books!.FindAsync(bookId, cancellationToken);
        if (book is { Quantity: > 0 })
        {
            book.Quantity++;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}