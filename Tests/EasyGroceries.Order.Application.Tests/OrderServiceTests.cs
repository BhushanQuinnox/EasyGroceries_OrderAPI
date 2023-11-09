


using AutoFixture;
using AutoMapper;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Commands;
using EasyGroceries.Order.Application.Features.OrderDetails.Requests.Queries;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Commands;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Queries;
using EasyGroceries.Order.Application.Profiles;
using EasyGroceries.Order.Application.Services;
using MediatR;
using Moq;

namespace EasyGroceries.Order.Application.Tests;

public class OrderServiceTests
{
    private readonly Mock<IMediator> _mediatorMock;

    public OrderServiceTests()
    {
        _mediatorMock = new Mock<IMediator>();
    }

    [Fact]
    public async Task GetOrderByUserId_Should_ReturnsOrderByUserId()
    {
        // Arrange
        var fixture = new Fixture();
        OrderHeaderDto orderHeaderDto = fixture.Create<OrderHeaderDto>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOrderHeaderByUserIdRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(orderHeaderDto);
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOrderDetailsByHeaderIdRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(orderHeaderDto.OrderDetails.ToList());

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        // Act
        OrderService orderService = new OrderService(_mediatorMock.Object, mockMapper.CreateMapper());
        var response = orderService.GetOrderByUserId(orderHeaderDto.UserId);

        // Assert
        _mediatorMock.Verify(x => x.Send(It.IsAny<GetOrderHeaderByUserIdRequest>(), It.IsAny<CancellationToken>()),
                             Times.Once);
        _mediatorMock.Verify(x => x.Send(It.IsAny<GetOrderDetailsByHeaderIdRequest>(), It.IsAny<CancellationToken>()),
                             Times.Once);
        Assert.Equal(orderHeaderDto, response.Result.Result);
    }

    [Fact]
    public async Task CreateOrder_Should_CreateOrderSuccessfully()
    {
        // Arrange
        var fixture = new Fixture();
        CartDto cartDto = fixture.Create<CartDto>();
        OrderHeaderDto orderHeaderDto = fixture.Create<OrderHeaderDto>();
        ResponseDto<OrderHeaderDto> orderHeaderResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = orderHeaderDto
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(orderHeaderResponse);

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderDetailsListRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        // Act
        OrderService orderService = new OrderService(_mediatorMock.Object, mockMapper.CreateMapper());
        var response = orderService.CreateOrder(cartDto);

        // Assert
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateOrderHeaderRequest>(), It.IsAny<CancellationToken>()),
                             Times.Once);
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateOrderDetailsListRequest>(), It.IsAny<CancellationToken>()),
                             Times.Once);
    }

    [Fact]
    public async Task CreateOrder_Should_FailedToCreateOrderAsFailedToCreateOrderHeader()
    {
        // Arrange
        var fixture = new Fixture();
        CartDto cartDto = fixture.Create<CartDto>();
        OrderHeaderDto orderHeaderDto = fixture.Create<OrderHeaderDto>();
        ResponseDto<OrderHeaderDto> orderHeaderResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = orderHeaderDto,
            IsSuccess = false
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(orderHeaderResponse);

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderDetailsListRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        // Act
        OrderService orderService = new OrderService(_mediatorMock.Object, mockMapper.CreateMapper());
        var response = orderService.CreateOrder(cartDto);

        // Assert
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateOrderHeaderRequest>(), It.IsAny<CancellationToken>()),
                             Times.Once);
        _mediatorMock.Verify(x => x.Send(It.IsAny<CreateOrderDetailsListRequest>(), It.IsAny<CancellationToken>()),
                             Times.Never);
    }

}