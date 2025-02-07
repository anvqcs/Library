using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Library.UseCases.Features.BorrowRecords.Commands.CreateBorrowRecord;

public record CreateBorrowRecordCommand : IRequest<int>
{
    public required int BookId { get; set; }
    public required string ApplicationUserId { get; set; }
    public double RentalCost { get; set; }
}

public class CreateBorrowRecordCommandHandler : IRequestHandler<CreateBorrowRecordCommand, int>
{
    private readonly IGenericRepository<BorrowRecord> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILibraryDbContext _context;

    public CreateBorrowRecordCommandHandler(IGenericRepository<BorrowRecord> repository, ILibraryDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<int> Handle(CreateBorrowRecordCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entity = new BorrowRecord
        {
            BookId = request.BookId,
            ApplicationUserId = request.ApplicationUserId,
            RentalCost = request.RentalCost,
            BorrowDate = DateTime.Now,
            Created = DateTime.Now,
            CreatedBy = userId
        };
        await UpdateBookQuantity(request.BookId, cancellationToken);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
    private async Task UpdateBookQuantity(int bookId, CancellationToken cancellationToken)
    {
        var book = await _context.Books!.FindAsync(bookId, cancellationToken);
        if (book is { Quantity: > 0 })
        {
            book.Quantity--;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}