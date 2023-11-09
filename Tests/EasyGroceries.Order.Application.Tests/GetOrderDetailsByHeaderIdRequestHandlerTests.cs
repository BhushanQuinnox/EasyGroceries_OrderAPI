

using AutoFixture;
using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.Features.OrderDetails.Handlers.Queries;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries;
using EasyGroceries.Order.Application.Profiles;
using EasyGroceries.Order.Domain;
using Moq;

namespace EasyGroceries.Order.Application.Tests;

public class GetOrderDetailsByHeaderIdRequestHandlerTests
{
    private readonly Mock<IOrderDetailsRepository> _orderDetailsRepositoryMock;

    public GetOrderDetailsByHeaderIdRequestHandlerTests()
    {
        _orderDetailsRepositoryMock = new Mock<IOrderDetailsRepository>();
    }

    [Fact]
    public async Task Handle_Should_ReturnsOrderDetailsOfSpecifiedHeaderId()
    {
        // Arrange
        var fixture = new Fixture();
        var orderDetails = fixture.Create<IReadOnlyList<OrderDetails>>();

        _orderDetailsRepositoryMock.Setup(x => x.GetOrderDetailsByOrderHeaderId(It.IsAny<int>()))
                                    .ReturnsAsync(orderDetails);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new GetOrderDetailsByHeaderIdRequest();
        var handler = new GetOrderDetailsByHeaderIdRequestHandler(_orderDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        _orderDetailsRepositoryMock.Verify(x => x.GetOrderDetailsByOrderHeaderId(It.IsAny<int>()), Times.Once);
        Assert.Equal(orderDetails.Count, result.Count);
        Assert.Equal(orderDetails.First().ProductName, result.First().ProductName);
    }
}