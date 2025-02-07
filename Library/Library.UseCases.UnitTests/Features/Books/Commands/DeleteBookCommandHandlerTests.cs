using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Books.Commands.DeleteBook;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.Books.Commands;

public class DeleteBookCommandHandlerTests
{
    private readonly IGenericRepository<Book> _repository;
    private readonly DeleteBookCommandHandler _handler;
    public DeleteBookCommandHandlerTests()
    {
        _repository = Substitute.For<IGenericRepository<Book>>();
        _handler = new DeleteBookCommandHandler(_repository);
    }
    [Fact]
    public async Task Handle_BookFound_CallDeleteAsync()
    {
        // Arrange
        int bookId = 1;
        var book = new Book { Id = bookId };
        _repository.GetByIdAsync(bookId, Arg.Any<CancellationToken>()).Returns(book);
        _repository.DeleteAsync(book, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        var command = new DeleteBookCommand(bookId);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _repository.Received(1).DeleteAsync(book, Arg.Any<CancellationToken>());
    }
    [Fact]
    public async Task Handle_BookNotFound_ThrowNotFoundException()
    {
        // Arrange
        int bookId = 1;
        _repository.GetByIdAsync(bookId, Arg.Any<CancellationToken>()).Returns((Book)null!);
        var command = new DeleteBookCommand(bookId);
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}