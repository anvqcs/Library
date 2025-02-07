using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Auths.Commands.SignIn;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.Auths.Commands;

public class SignInCommandHandlerTests
{
    private readonly SignInCommandHandler _handler;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<ApplicationUser> _userManager;

    public SignInCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null);
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new SignInCommandHandler(_userManager, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsToken()
    {
        // Arrange
        var command = new SignInCommand { Email = "test@example.com", Password = "Password123" };
        var user = new ApplicationUser { Email = command.Email };
        _userManager.FindByEmailAsync(command.Email)!.Returns(Task.FromResult(user));
        _userManager.CheckPasswordAsync(user, command.Password).Returns(Task.FromResult(true));
        _jwtTokenGenerator.GenerateTokenAsync(user).Returns(Task.FromResult("valid-token"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("valid-token", result.Token);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = new SignInCommand { Email = "test@example.com", Password = "WrongPassword" };
        var user = new ApplicationUser { Email = command.Email };
        _userManager.FindByEmailAsync(command.Email)!.Returns(Task.FromResult(user));
        _userManager.CheckPasswordAsync(user, command.Password).Returns(Task.FromResult(false));

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
    }
}