using System.Security.Claims;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Books.Commands.CreateBook;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.Books.Commands;

public class CreateBookCommandHandlerTests
{
    private readonly CreateBookCommandHandler _handler;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<Book> _repository;

    public CreateBookCommandHandlerTests()
    {
        _repository = Substitute.For<IGenericRepository<Book>>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _handler = new CreateBookCommandHandler(_repository, _httpContextAccessor);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsBookId()
    {
        // Arrange
        var command = new CreateBookCommand
        {
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
        _repository.AddAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(book));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(book.Id, result);
        await _repository.Received(1).AddAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>());
    }
}