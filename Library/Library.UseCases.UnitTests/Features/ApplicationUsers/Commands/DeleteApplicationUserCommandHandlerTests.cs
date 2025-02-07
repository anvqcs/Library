using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.UseCases.Features.ApplicationUsers.Commands.DeleteApplicationUser;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.ApplicationUsers.Commands;

public class DeleteApplicationUserCommandHandlerTests
{
    private readonly DeleteApplicationUserCommandHandler _handler;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteApplicationUserCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _handler = new DeleteApplicationUserCommandHandler(_userManager);
    }

    [Fact]
    public async Task Handle_UserFound_ShouldMarkUserAsDeleted()
    {
        // Arrange
        var userId = "test-user-id";
        var user = new ApplicationUser { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.UpdateAsync(user).Returns(IdentityResult.Success);

        var command = new DeleteApplicationUserCommand(userId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(user.IsDeleted);
        await _userManager.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = "non-existent-user-id";
        _userManager.FindByIdAsync(userId).Returns((ApplicationUser)null!);

        var command = new DeleteApplicationUserCommand(userId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}