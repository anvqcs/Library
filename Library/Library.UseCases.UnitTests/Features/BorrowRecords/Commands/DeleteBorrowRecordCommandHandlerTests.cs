using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.BorrowRecords.Commands.DeleteBorrowRecord;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.BorrowRecords.Commands;

public class DeleteBorrowRecordCommandHandlerTests
{
    private readonly IGenericRepository<BorrowRecord> _repository;
    private readonly DeleteBorrowRecordCommandHandler _handler;
    public DeleteBorrowRecordCommandHandlerTests()
    {
        _repository = Substitute.For<IGenericRepository<BorrowRecord>>();
        _handler = new DeleteBorrowRecordCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_BorrowRecordFound_CallDeleteAsync()
    {
        //Arrange
        int borrowRecordId = 1;
        var borrowRecord = new BorrowRecord { Id = borrowRecordId };
        _repository.GetByIdAsync(borrowRecordId, Arg.Any<CancellationToken>()).Returns(borrowRecord);
        _repository.DeleteAsync(borrowRecord, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        var command = new DeleteBorrowRecordCommand(borrowRecordId);

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _repository.Received(1).DeleteAsync(borrowRecord, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_BorrowRecordNotFound_ThrowNotFoundException()
    {
        //Arrange
        int borrowRecordId = 1;
        _repository.GetByIdAsync(borrowRecordId, Arg.Any<CancellationToken>()).Returns((BorrowRecord)null!);
        var command = new DeleteBorrowRecordCommand(borrowRecordId);

        //Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, Arg.Any<CancellationToken>()));
    }
}