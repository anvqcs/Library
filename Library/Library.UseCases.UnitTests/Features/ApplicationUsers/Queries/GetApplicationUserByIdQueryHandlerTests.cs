using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.UseCases.Features.ApplicationUsers.Queries.GetApplicationUserById;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.ApplicationUsers.Queries;

public class GetApplicationUserByIdQueryHandlerTests
{
    private readonly GetApplicationUserByIdQueryHandler _handler;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetApplicationUserByIdQueryHandlerTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _handler = new GetApplicationUserByIdQueryHandler(_userManager);
    }

    [Fact]
    public async Task Handle_UserFound_ShouldReturnUser()
    {
        // Arrange
        var userId = "test-user-id";
        var user = new ApplicationUser { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);

        var query = new GetApplicationUserByIdQuery { id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = "non-existent-user-id";
        _userManager.FindByIdAsync(userId).Returns((ApplicationUser)null!);

        var query = new GetApplicationUserByIdQuery { id = userId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}