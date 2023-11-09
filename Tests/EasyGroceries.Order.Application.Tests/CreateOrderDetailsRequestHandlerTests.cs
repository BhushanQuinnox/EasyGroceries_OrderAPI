

using System.Net;
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

public class CreateOrderDetailsRequestHandlerTests
{
    private readonly Mock<IOrderDetailsRepository> _orderDetailsRepositoryMock;

    public CreateOrderDetailsRequestHandlerTests()
    {
        _orderDetailsRepositoryMock = new Mock<IOrderDetailsRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateOrderDetailsSuccessfully()
    {
        // Arrange
        var fixture = new Fixture();
        var orderDetailsDto = fixture.Create<OrderDetailsDto>();
        var orderDetails = fixture.Create<OrderDetails>();

        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        _orderDetailsRepositoryMock.Setup(x => x.Add(It.IsAny<OrderDetails>()))
                                    .Callback(() =>
                                    {
                                        orderDetailsList.Add(orderDetails);
                                    });

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new CreateOrderDetailsRequest() { OrderDetailsDto = orderDetailsDto };
        var handler = new CreateOrderDetailsRequestHandler(_orderDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var actualResponse = handler.Handle(command, default);

        // Assert
        _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()),
              Times.Once);
    }

    [Fact]
    public async Task Handle_Should_FailedToCreateOrderDetailsAsValidationFailed()
    {
        // Arrange
        var fixture = new Fixture();
        var orderDetailsDto = fixture.Create<OrderDetailsDto>();
        var orderDetails = fixture.Create<OrderDetails>();

        // Set Price to zero as it fail in validation
        orderDetailsDto.Price = 0;

        ResponseDto<OrderDetailsDto> expectedResponse = new ResponseDto<OrderDetailsDto>()
        {
            Result = orderDetailsDto,
            IsSuccess = true,
            Status = (int)HttpStatusCode.OK
        };

        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        _orderDetailsRepositoryMock.Setup(x => x.Add(It.IsAny<OrderDetails>()))
                                    .Callback(() =>
                                    {
                                        orderDetailsList.Add(orderDetails);
                                    });

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new CreateOrderDetailsRequest() { OrderDetailsDto = orderDetailsDto };
        var handler = new CreateOrderDetailsRequestHandler(_orderDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var actualResponse = handler.Handle(command, default);

        // Assert
        _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()),
              Times.Never);
    }

}