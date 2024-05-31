namespace Tests.Validators
{
    public class CreateSubscriptionCommandValidatorTests
    {
        [Fact]
        public void Validate_ValidCreateSubscriptionCommand_ReturnsTrue()
        {
            // Arrange
            var validator = new CreateSubscriptionCommandValidator();
            var command = new CreateSubscriptionCommand(Guid.NewGuid());

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "IdUser required.")]
        public void Validate_InvalidCreateSubscriptionCommand_ReturnsFalse(string idUser, string expectedErrorMessage)
        {
            // Arrange
            var validator = new CreateSubscriptionCommandValidator();
            var command = new CreateSubscriptionCommand(Guid.Parse(idUser));

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}