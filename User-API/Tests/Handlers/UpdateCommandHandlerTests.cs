namespace Tests.Handlers
{
    public class UpdateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidUpdate_ReturnsSuccessResponse()
        {
            // Arrange
            var email = "test@example.com";
            var newEmail = "newtest@example.com";
            var updateUserCommand = new UpdateUserCommand(email, newEmail);
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new UpdateUserCommandValidator();
            var handler = new UpdateUserCommandHandler(validator, userRepositoryMock.Object);

            userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(new User { Email = email });

            // Act
            var result = await handler.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Successfully updated.");
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var email = "test@example.com";
            var newEmail = "newtest@example.com";
            var updateUserCommand = new UpdateUserCommand(email, newEmail);
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new UpdateUserCommandValidator();
            var handler = new UpdateUserCommandHandler(validator, userRepositoryMock.Object);

            userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync((User)null);

            // Act
            var result = await handler.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("You are not registered.");
        }
    }
}
