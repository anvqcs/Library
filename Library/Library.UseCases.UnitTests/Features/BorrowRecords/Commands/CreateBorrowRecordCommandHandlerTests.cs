using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.BorrowRecords.Commands.CreateBorrowRecord;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;

namespace Library.UseCases.UnitTests.Features.BorrowRecords.Commands;

public class CreateBorrowRecordCommandHandlerTests
{
    private readonly IGenericRepository<BorrowRecord> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly ILibraryDbContext _context;
    private readonly CreateBorrowRecordCommandHandler _handler;
    public CreateBorrowRecordCommandHandlerTests()
    {
        _repository = Substitute.For<IGenericRepository<BorrowRecord>>();
        _context = Substitute.For<ILibraryDbContext>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _handler = new CreateBorrowRecordCommandHandler(_repository, _context, _httpContextAccessor);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsBorrowRecordId()
    {
        //Arrange
        var command = new CreateBorrowRecordCommand()
        {
            BookId = 1,
            ApplicationUserId = Guid.NewGuid().ToString(),
            RentalCost = 10.00
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
            BorrowDate = DateTime.Now,
            Created = DateTime.Now,
            CreatedBy = userId
        };
        _repository.AddAsync(Arg.Any<BorrowRecord>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(borrowRecord));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(borrowRecord.Id, result);
        await _repository.Received(1).AddAsync(Arg.Any<BorrowRecord>(), Arg.Any<CancellationToken>());
    }
}