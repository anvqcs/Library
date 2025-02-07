using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Books.Commands.UpdateBook;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;
using Library.Core.Common.Exceptions;

namespace Library.UseCases.UnitTests.Features.Books.Commands;

public class UpdateBookCommandHandlerTests
{
    private readonly IGenericRepository<Book> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UpdateBookCommandHandler _handler;
    public UpdateBookCommandHandlerTests()
    {
        _repository = Substitute.For<IGenericRepository<Book>>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _handler = new UpdateBookCommandHandler(_repository, _httpContextAccessor);
    }

    [Fact]
    public async Task Handle_BookFound_CallUpdateAsync()
    {
        // Arrange
        var command = new UpdateBookCommand
        {
            Id = 1,
            Title = "Book Title",
            Author = "Book Author",
            Year = 2022,
            ISBN = "1234567890",
            Description = "Book Description",
            Quantity = 10,
            RentalPrice = 10.00,
            GenreId = 1
        };
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, userId)
        }));
        _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = claimsPrincipal });

        var book = new Book
        {
            Title = command.Title,
            Author = command.Author,
            Year = command.Year,
            ISBN = command.ISBN,
            Description = command.Description,
            Quantity = command.Quantity,
            RentalPrice = command.RentalPrice,
            GenreId = command.GenreId,
            CreatedBy = userId,
            Created = DateTime.Now
        };
        _repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(book);
        _repository.UpdateAsync(book, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _repository.Received(1).UpdateAsync(book, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_BookNotFound_ThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateBookCommand
        {
            Id = 100,
            Title = "Book Title",
            Author = "Book Author",
            Year = 2022,
            ISBN = "1234567890",
            Description = "Book Description",
            Quantity = 10,
            RentalPrice = 10.00,
            GenreId = 1
        };
        _repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Book)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, Arg.Any<CancellationToken>()));
    }
}