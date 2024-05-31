namespace Tests.Handlers
{
    public class SignUpCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidSignUpCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new SignUpCommandValidator();
            var jwtProviderMock = new Mock<IJwtProvider>();

            var handler = new SignUpCommandHandler(userRepositoryMock.Object, validator, jwtProviderMock.Object);
            var command = new SignUpCommand("Test User", "test@example.com", "password123");

            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<User>()));
            jwtProviderMock.Setup(x => x.GenerateAsync(It.IsAny<User>())).ReturnsAsync("jwt_token");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("User successfully created.");
            result.Data.Should().Be("jwt_token");
        }

        [Fact]
        public async Task Handle_SignUpCommandWithEmailAlreadyExists_ReturnsFailureResponse()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new SignUpCommandValidator();
            var jwtProviderMock = new Mock<IJwtProvider>();

            var handler = new SignUpCommandHandler(userRepositoryMock.Object, validator, jwtProviderMock.Object);
            var command = new SignUpCommand("Test User", "test@example.com", "password123");

            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("This email already exists.");
        }

        [Fact]
        public async Task Handle_ExceptionSavingUser_ReturnsFailureResponse()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new SignUpCommandValidator();
            var jwtProviderMock = new Mock<IJwtProvider>();

            var handler = new SignUpCommandHandler(userRepositoryMock.Object, validator, jwtProviderMock.Object);
            var command = new SignUpCommand("Test User", "test@example.com", "password123");

            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Failed to save user."));

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<Exception>().WithMessage("Failed to save user.");
        }

        [Fact]
        public async Task Handle_ExceptionGeneratingJwtToken_ReturnsFailureResponse()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new SignUpCommandValidator();
            var jwtProviderMock = new Mock<IJwtProvider>();

            var handler = new SignUpCommandHandler(userRepositoryMock.Object, validator, jwtProviderMock.Object);
            var command = new SignUpCommand("Test User", "test@example.com", "password123");

            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<User>()));
            jwtProviderMock.Setup(x => x.GenerateAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Failed to generate JWT token."));

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<Exception>().WithMessage("Failed to generate JWT token.");
        }
    }
}
