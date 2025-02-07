using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.UseCases.Features.ApplicationUsers.Commands.UpdateApplicationUser;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.ApplicationUsers.Commands;

public class UpdateApplicationUserCommandHandlerTests
{
    private readonly UpdateApplicationUserCommandHandler _handler;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateApplicationUserCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _handler = new UpdateApplicationUserCommandHandler(_userManager);
    }

    [Fact]
    public async Task Handle_UserFound_ShouldUpdateUserDetails()
    {
        // Arrange
        var userId = "test-user-id";
        var user = new ApplicationUser { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.UpdateAsync(user).Returns(IdentityResult.Success);

        var command = new UpdateApplicationUserCommand
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("john.doe@example.com", user.Email);
        Assert.Equal("1234567890", user.PhoneNumber);
        Assert.Equal("123 Main St", user.Address);
        await _userManager.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = "non-existent-user-id";
        _userManager.FindByIdAsync(userId).Returns((ApplicationUser)null!);

        var command = new UpdateApplicationUserCommand
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}