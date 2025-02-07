using System.Security.Claims;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.BorrowRecords.Commands.UpdateBorrowRecord;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.BorrowRecords.Commands;

public class UpdateBorrowRecordCommandHandlerTests
{
    private readonly IGenericRepository<BorrowRecord> _repository;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly ILibraryDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UpdateBorrowRecordCommandHandler _handler;
    public UpdateBorrowRecordCommandHandlerTests()
    {
        _repository = Substitute.For<IGenericRepository<BorrowRecord>>();
        _context = Substitute.For<ILibraryDbContext>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _handler = new UpdateBorrowRecordCommandHandler(_repository, _context, _httpContextAccessor);
    }

    [Fact]
    public async Task Handle_BorrowRecordFound_CallUpdateAsync()
    {
        var command = new UpdateBorrowRecordCommand()
        {
            Id = 1,
            BookId = 1,
            ApplicationUserId = Guid.NewGuid().ToString(),
            RentalCost = 10.0,
            IsReturned = false
        };
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, userId)
        }));
        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = claimsPrincipal });

        var borrowRecord = new BorrowRecord()
        {
            BookId = command.BookId,
            ApplicationUserId = command.ApplicationUserId,
            RentalCost = command.RentalCost,
            Modified = DateTime.Now,
            ModifiedBy = userId
        };
        _repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(borrowRecord);
        _repository.UpdateAsync(borrowRecord, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _repository.Received(1).UpdateAsync(borrowRecord, Arg.Any<CancellationToken>());
    }
}