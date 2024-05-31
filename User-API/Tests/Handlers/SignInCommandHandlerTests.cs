namespace Tests.Handlers
{
    public class SignInCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidSignIn_ReturnsSuccessResponse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = User.Create("Test User", email, password);
            var token = "generated_jwt_token";

            var saltBytes = Encoding.UTF8.GetBytes("salt_value");

            var userRepositoryMock = new Mock<IUserRepository>();
            var jwtProviderMock = new Mock<IJwtProvider>();
            var validator = new SignInCommandValidator();

            var handler = new SignInCommandHandler(jwtProviderMock.Object, validator, userRepositoryMock.Object);
            var command = new SignInCommand(email, password);

            var passwordInstance = new Password(password, saltBytes);
            var hashedPassword = passwordInstance.Value;

            userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.GetPasswordByIdUserAsync(user.Id)).ReturnsAsync(hashedPassword);
            userRepositoryMock.Setup(x => x.GetSaltByIdUserAsync(user.Id)).ReturnsAsync(saltBytes);

            jwtProviderMock.Setup(x => x.GenerateAsync(user)).ReturnsAsync(token);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().BeNullOrEmpty();
            result.Data.Should().Be(token);
        }


        [Fact]
        public async Task Handle_InvalidEmail_ReturnsFailureResponse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var invalidEmail = "invalid@example.com";

            var userRepositoryMock = new Mock<IUserRepository>();
            var jwtProviderMock = new Mock<IJwtProvider>();
            var validator = new SignInCommandValidator();

            var handler = new SignInCommandHandler(jwtProviderMock.Object, validator, userRepositoryMock.Object);
            var command = new SignInCommand(invalidEmail, password);

            userRepositoryMock.Setup(x => x.GetByEmailAsync(invalidEmail)).ReturnsAsync((User)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Your email or password is incorrect.");
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailureResponse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = User.Create("Test User", email, password);
            var invalidPassword = "wrong_password";

            var userRepositoryMock = new Mock<IUserRepository>();
            var jwtProviderMock = new Mock<IJwtProvider>();
            var validator = new SignInCommandValidator();

            var handler = new SignInCommandHandler(jwtProviderMock.Object, validator, userRepositoryMock.Object);
            var command = new SignInCommand(email, invalidPassword);

            userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.GetPasswordByIdUserAsync(user.Id)).ReturnsAsync(password);
            userRepositoryMock.Setup(x => x.GetSaltByIdUserAsync(user.Id)).ReturnsAsync(Encoding.UTF8.GetBytes("salt_value"));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Your email or password is incorrect.");
        }
    }
}
