using AutoFixture;
using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.Features.OrderDetails.Handlers.Queries;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries;
using EasyGroceries.Order.Application.Profiles;
using EasyGroceries.Order.Domain;
using Moq;

namespace EasyGroceries.Order.Application.Tests;

public class GetOrderDetailsRequestHandlerTests
{
    private readonly Mock<IOrderDetailsRepository> _orderDetailsRepositoryMock;

    public GetOrderDetailsRequestHandlerTests()
    {
        _orderDetailsRepositoryMock = new Mock<IOrderDetailsRepository>();
    }

    [Fact]
    public async Task Handle_Should_ReturnsAllOrderDetails()
    {
        // Arrange
        var fixture = new Fixture();
        var orderDetails = fixture.Create<List<OrderDetails>>();

        _orderDetailsRepositoryMock.Setup(x => x.GetAllOrderDetails())
                                    .ReturnsAsync(orderDetails);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new GetOrderDetailsRequest();
        var handler = new GetOrderDetailsRequestHandler(_orderDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        _orderDetailsRepositoryMock.Verify(x => x.GetAllOrderDetails(), Times.Once);
        Assert.Equal(orderDetails.Count, result.Count);
    }
}