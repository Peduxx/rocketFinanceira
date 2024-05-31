namespace Tests.Validators
{
    public class SignUpCommandValidatorTests
    {
        [Fact]
        public void Validate_ValidSignUpCommand_ReturnsTrue()
        {
            // Arrange
            var validator = new SignUpCommandValidator();
            var command = new SignUpCommand("Test User", "test@example.com", "password123");

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "test@example.com", "password123", new[] { "Name is required." })]
        [InlineData("Test User", "", "password123", new[] { "Invalid email address." })]
        [InlineData("Test User", "invalidemail", "password123", new[] { "Invalid email address." })]
        [InlineData("", "test@example.com", "", new[] { "Name is required.", "Password is required." })]
        [InlineData("Test User", "test@example.com", "123", new[] { "The password must be at least 6 characters long." })]
        public void Validate_InvalidSignUpCommand_ReturnsFalse(string name, string email, string password, string[] expectedErrorMessages)
        {
            // Arrange
            var validator = new SignUpCommandValidator();
            var command = new SignUpCommand(name, email, password);

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorMessage).Should().Contain(expectedErrorMessages);
        }
    }
}
