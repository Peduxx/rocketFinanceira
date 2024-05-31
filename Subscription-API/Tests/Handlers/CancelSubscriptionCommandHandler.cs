namespace Tests.Handlers
{
    public class CancelSubscriptionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_Success()
        {
            // Arrange
            var command = new CancelSubscriptionCommand(Guid.NewGuid());
            var validator = new CancelSubscriptionCommandValidator();

            var mockRepository = new Mock<ISubscriptionRepository>();
            mockRepository.Setup(r => r.GetActiveSubscriptionAsync(command.IdUser))
                          .ReturnsAsync(true);

            var handler = new CancelSubscriptionCommandHandler(validator, mockRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }
    }

}
