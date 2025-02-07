using AutoMapper;
using Library.Core.Common.Models;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Books.Queries.GetBooksWithPagination;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.Books.Queries;

public class GetBooksWithPaginationQueryTests
{
    private readonly GetTodoItemsWithPaginationQueryHandler _handler;
    private readonly ILibraryDbContext _mockDbContext;
    private readonly IMapper _mockMapper;

    public GetBooksWithPaginationQueryTests()
    {
        _mockDbContext = Substitute.For<ILibraryDbContext>();
        _mockMapper = Substitute.For<IMapper>();
        _handler = new GetTodoItemsWithPaginationQueryHandler(_mockDbContext, _mockMapper);
    }

    [Fact]
    public void Handle_ValidRequest_ReturnsPaginatedList()
    {
        // Arrange
        // ReSharper disable once UnusedVariable
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Book 1", Author = "Author 1", GenreId = 1 },
            new() { Id = 2, Title = "Book 2", Author = "Author 2", GenreId = 2 }
        }.AsQueryable();

        // ReSharper disable once UnusedVariable
        var paginatedBooks = new PaginatedList<BookBriefDto>(new List<BookBriefDto>(), 2, 1, 10);

        var mockSet = Substitute.For<DbSet<Book>, IQueryable<Book>>();
        _mockDbContext.Books.Returns(mockSet);
        _mockMapper.Map<BookBriefDto>(Arg.Any<Book>()).Returns(new BookBriefDto());

        var query = new GetBooksWithPaginationQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}