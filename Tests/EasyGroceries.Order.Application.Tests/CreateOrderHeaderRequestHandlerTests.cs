using System.Net;
using AutoFixture;
using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderHeader.Handlers.Commands;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Commands;
using EasyGroceries.Order.Application.Profiles;
using EasyGroceries.Order.Domain;
using Moq;

namespace EasyGroceries.Order.Application.Tests;

public class CreateOrderHeaderRequestHandlerTests
{
    private readonly Mock<IOrderHeaderRepository> _orderHeaderRepositoryMock;

    public CreateOrderHeaderRequestHandlerTests()
    {
        _orderHeaderRepositoryMock = new Mock<IOrderHeaderRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateOrderHeaderSuccessfully()
    {
        // Arrange
        var fixture = new Fixture();
        var orderHeaderDto = fixture.Create<OrderHeaderDto>();
        var orderHeader = fixture.Create<OrderHeader>();
        ResponseDto<OrderHeaderDto> expectedResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = orderHeaderDto,
            IsSuccess = true,
            Status = (int)HttpStatusCode.OK
        };

        List<OrderHeader> orderHeaderList = new List<OrderHeader>();
        _orderHeaderRepositoryMock.Setup(x => x.Add(It.IsAny<OrderHeader>()))
                                    .Callback(() =>
                                    {
                                        orderHeaderList.Add(orderHeader);
                                    });

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new CreateOrderHeaderRequest() { OrderHeaderDto = orderHeaderDto };
        var handler = new CreateOrderHeaderRequestHandler(_orderHeaderRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var actualResponse = handler.Handle(command, default);

        // Assert
        _orderHeaderRepositoryMock.Verify(x => x.Add(It.IsAny<OrderHeader>()),
              Times.Once);
    }

    [Fact]
    public async Task Handle_Should_FailedToCreateOrderHeaderAsValidationFailed()
    {
        // Arrange
        var fixture = new Fixture();
        var orderHeaderDto = fixture.Create<OrderHeaderDto>();
        var orderHeader = fixture.Create<OrderHeader>();

        // Setting UserId to zero to fail in validation.
        orderHeaderDto.UserId = 0;

        ResponseDto<OrderHeaderDto> expectedResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = orderHeaderDto,
            IsSuccess = true,
            Status = (int)HttpStatusCode.OK
        };

        List<OrderHeader> orderHeaderList = new List<OrderHeader>();
        _orderHeaderRepositoryMock.Setup(x => x.Add(It.IsAny<OrderHeader>()))
                                    .Callback(() =>
                                    {
                                        orderHeaderList.Add(orderHeader);
                                    });

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var command = new CreateOrderHeaderRequest() { OrderHeaderDto = orderHeaderDto };
        var handler = new CreateOrderHeaderRequestHandler(_orderHeaderRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var actualResponse = handler.Handle(command, default);

        // Assert
        _orderHeaderRepositoryMock.Verify(x => x.Add(It.IsAny<OrderHeader>()),
              Times.Never);
    }

}