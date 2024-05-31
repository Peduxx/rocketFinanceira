namespace Tests.Validators
{
    public class SignInCommandValidatorTests
    {
        [Fact]
        public void Validate_ValidSignInCommand_ReturnsTrue()
        {
            // Arrange
            var validator = new SignInCommandValidator();
            var command = new SignInCommand("test@example.com", "password123");

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "password123", "Invalid email address.")]
        [InlineData("test@example.com", "", "The password cannot be empty.")]
        [InlineData("invalidemail", "password123", "Invalid email address.")]
        public void Validate_InvalidSignInCommand_ReturnsFalse(string email, string password, string expectedErrorMessage)
        {
            // Arrange
            var validator = new SignInCommandValidator();
            var command = new SignInCommand(email, password);

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}
