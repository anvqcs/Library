using Library.Core.Entities;
using Library.UseCases.Features.ApplicationUsers.Queries.GetApplicationUsersWithPagination;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.ApplicationUsers.Queries;

public class GetApplicationUsersWithPaginationQueryHandlerTests
{
    private readonly GetApplicationUsersWithPaginationQueryHandler _handler;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetApplicationUsersWithPaginationQueryHandlerTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _handler = new GetApplicationUsersWithPaginationQueryHandler(_userManager);
    }

    [Fact]
    public void Handle_ValidRequest_ShouldReturnPaginatedList()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new() { Id = "1", UserName = "user1" },
            new() { Id = "2", UserName = "user2" }
        }.AsQueryable();

        _userManager.Users.Returns(users);

        var query = new GetApplicationUsersWithPaginationQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}