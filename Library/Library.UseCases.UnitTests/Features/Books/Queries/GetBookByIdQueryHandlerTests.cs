using AutoMapper;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Books.Queries.GetBookById;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.Books.Queries;

public class GetBookByIdQueryHandlerTests
{
    private readonly ILibraryDbContext _context;
    private readonly GetBookByIdQueryHandler _handler;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandlerTests()
    {
        _context = Substitute.For<ILibraryDbContext>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetBookByIdQueryHandler(_context, _mapper);
    }

    [Fact]
    public void Handle_BookExists_ReturnsBookDto()
    {
        // Arrange
        var bookId = 1;
        var book = new Book { Id = bookId, Title = "Test Book" };
        var bookDto = new BookDto { Id = bookId, Title = "Test Book" };

        var dbSet = Substitute.For<DbSet<Book>, IQueryable<Book>>();
        dbSet.FindAsync(Arg.Any<object[]>()).Returns(book);
        _context.Books.Returns(dbSet);
        _mapper.Map<BookDto>(book).Returns(bookDto);

        var query = new GetBookByIdQuery { Id = bookId };
        // Act
        var result = _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookId, result.Id);
    }
}