namespace Tests.Validators
{
    public class UpdateUserCommandValidatorTests
    {
        [Fact]
        public void Validate_ValidUpdateUserCommand_ReturnsTrue()
        {
            // Arrange
            var validator = new UpdateUserCommandValidator();
            var command = new UpdateUserCommand("test@example.com", "newtest@example.com");

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "newtest@example.com", "Invalid email address.")]
        [InlineData("test@example.com", "", "Invalid email address.")]
        [InlineData("invalidemail", "newtest@example.com", "Invalid email address.")]
        [InlineData("test@example.com", "invalidemail", "Invalid email address.")]
        public void Validate_InvalidUpdateUserCommand_ReturnsFalse(string email, string newEmail, string expectedErrorMessage)
        {
            // Arrange
            var validator = new UpdateUserCommandValidator();
            var command = new UpdateUserCommand(email, newEmail);

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}
