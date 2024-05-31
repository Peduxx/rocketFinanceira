namespace Tests.Validators
{
    public class CancelSubscriptionCommandValidatorTests
    {
        [Fact]
        public void Validate_ValidCancelSubscriptionCommand_ReturnsTrue()
        {
            // Arrange
            var validator = new CancelSubscriptionCommandValidator();
            var command = new CancelSubscriptionCommand(Guid.NewGuid());

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "IdUser required.")]
        public void Validate_InvalidCancelSubscriptionCommand_ReturnsFalse(string idUser, string expectedErrorMessage)
        {
            // Arrange
            var validator = new CancelSubscriptionCommandValidator();
            var command = new CancelSubscriptionCommand(Guid.Parse(idUser));

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}