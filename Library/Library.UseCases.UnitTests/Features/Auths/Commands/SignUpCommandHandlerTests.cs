using Library.Core.Constants;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.UseCases.Features.Auths.Commands.SignUp;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Library.UseCases.UnitTests.Features.Auths.Commands;

public class SignUpCommandHandlerTests
{
    private readonly SignUpCommandHandler _handler;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SignUpCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null);
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(
            Substitute.For<IRoleStore<IdentityRole>>(), null, null, null, null);
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new SignUpCommandHandler(_userManager, _roleManager, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsToken()
    {
        // Arrange
        var command = new SignUpCommand
        {
            Email = "test@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
            FirstName = "John",
            LastName = "Doe"
        };
        _userManager.FindByNameAsync(command.Email).Returns((ApplicationUser)null!);
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), command.Password).Returns(IdentityResult.Success);
        _roleManager.RoleExistsAsync(Roles.Reader).Returns(true);
        _jwtTokenGenerator.GenerateTokenAsync(Arg.Any<ApplicationUser>()).Returns(Task.FromResult("valid-token"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("valid-token", result.Token);
    }

    [Fact]
    public async Task Handle_PasswordAndConfirmPasswordDoNotMatch_ThrowsArgumentException()
    {
        // Arrange
        var command = new SignUpCommand
        {
            Email = "test@example.com",
            Password = "Password123",
            ConfirmPassword = "Password456",
            FirstName = "John",
            LastName = "Doe"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UserAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var command = new SignUpCommand
        {
            Email = "test@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
            FirstName = "John",
            LastName = "Doe"
        };
        _userManager.FindByNameAsync(command.Email).Returns(new ApplicationUser());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DefaultRoleDoesNotExists_CallsCreateRoleAndAddToRole()
    {
        var command = new SignUpCommand
        {
            Email = "test@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
            FirstName = "John",
            LastName = "Doe"
        };

        _userManager.FindByNameAsync(command.Email).Returns((ApplicationUser)null!);
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), command.Password).Returns(IdentityResult.Success);
        _roleManager.RoleExistsAsync(Roles.Reader).Returns(false);
        _roleManager.CreateAsync(Arg.Any<IdentityRole>()).Returns(IdentityResult.Success);
        _jwtTokenGenerator.GenerateTokenAsync(Arg.Any<ApplicationUser>()).Returns("token");

        // ReSharper disable once UnusedVariable
        var result = await _handler.Handle(command, CancellationToken.None);

        await _roleManager.Received().CreateAsync(Arg.Is<IdentityRole>(r => r.Name == Roles.Reader));
        await _userManager.Received().AddToRoleAsync(Arg.Any<ApplicationUser>(), Roles.Reader);
    }
}