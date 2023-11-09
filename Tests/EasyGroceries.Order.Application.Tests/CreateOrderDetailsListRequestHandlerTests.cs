

using AutoFixture;
using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderDetails.Handlers.Commands;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Commands;
using EasyGroceries.Order.Application.Profiles;
using EasyGroceries.Order.Domain;
using Moq;

namespace EasyGroceries.Order.Application.Tests;

public class CreateOrderDetailsListRequestHandlerTests
{
    private readonly Mock<IOrderDetailsRepository> _orderDetailsRepositoryMock;

    public CreateOrderDetailsListRequestHandlerTests()
    {
        _orderDetailsRepositoryMock = new Mock<IOrderDetailsRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateOrderDetailsListSuccessfully()
    {
        // Arrange
        var fixture = new Fixture();
        var orderDetailsDto = fixture.Create<List<OrderDetailsDto>>();
        var orderDetails = fixture.Create<List<OrderDetails>>();

        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        _orderDetailsRepositoryMock.Setup(x => x.AddOrderDetailsList(It.IsAny<List<OrderDetails>>()))
                                    .Callback(() =>
                                    {
                                        orderDetailsList.AddRange(orderDetails);
                                    });

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new CreateOrderDetailsListRequest() { OrderDetailsDtoLst = orderDetailsDto };
        var handler = new CreateOrderDetailsListRequestHandler(_orderDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var actualResponse = handler.Handle(command, default);

        // Assert
        _orderDetailsRepositoryMock.Verify(x => x.AddOrderDetailsList(It.IsAny<List<OrderDetails>>()),
              Times.Once);
    }
}