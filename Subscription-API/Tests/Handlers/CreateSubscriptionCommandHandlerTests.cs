namespace Tests.Handlers
{
    public class CreateSubscriptionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_Success()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(Guid.NewGuid());
            var validator = new CreateSubscriptionCommandValidator();

            var mockRepository = new Mock<ISubscriptionRepository>();
            mockRepository.Setup(r => r.GetActiveSubscriptionAsync(command.IdUser))
                          .ReturnsAsync(false);

            var mockBillingService = new Mock<IBillingService>();
            mockBillingService.Setup(bs => bs.ProcessBillingRequest(It.IsAny<Subscription>()))
                              .ReturnsAsync(true);

            var handler = new CreateSubscriptionCommandHandler(mockRepository.Object, mockBillingService.Object, validator);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle_AlreadySubscribed_ReturnFailure()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(Guid.NewGuid());
            var validator = new CreateSubscriptionCommandValidator();

            var mockRepository = new Mock<ISubscriptionRepository>();
            mockRepository.Setup(r => r.GetActiveSubscriptionAsync(command.IdUser))
                          .ReturnsAsync(true);

            var mockBillingService = new Mock<IBillingService>();

            var handler = new CreateSubscriptionCommandHandler(mockRepository.Object, mockBillingService.Object, validator);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Handle_BillingServiceFails_ReturnFailure()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(Guid.NewGuid());
            var validator = new CreateSubscriptionCommandValidator();

            var mockRepository = new Mock<ISubscriptionRepository>();
            mockRepository.Setup(r => r.GetActiveSubscriptionAsync(command.IdUser))
                          .ReturnsAsync(false);

            var mockBillingService = new Mock<IBillingService>();
            mockBillingService.Setup(bs => bs.ProcessBillingRequest(It.IsAny<Subscription>()))
                              .ReturnsAsync(false);

            var handler = new CreateSubscriptionCommandHandler(mockRepository.Object, mockBillingService.Object, validator);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
        }
    }
}
