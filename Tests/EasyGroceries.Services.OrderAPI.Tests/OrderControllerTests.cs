using AutoFixture;
using EasyGroceries.Order.Application.Contracts.MessageBus;
using EasyGroceries.Order.Application.Contracts.Services;
using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Services.OrderAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EasyGroceries.Services.OrderAPI.Tests;

public class OrderControllerTests
{
    private Mock<IOrderService> _orderServiceMock;
    private Mock<IMessageBus> _messageBusMock;
    private Mock<IConfiguration> _configurationMock;

    public OrderControllerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _messageBusMock = new Mock<IMessageBus>();
        _configurationMock = new Mock<IConfiguration>();
    }

    [Fact]
    public async void Get_Should_ReturnOrderHeaderOfSpecifiedUserId()
    {
        // Arrange
        Fixture fixture = new Fixture();
        OrderHeaderDto orderHeaderDto = fixture.Create<OrderHeaderDto>();
        ResponseDto<OrderHeaderDto> expectedResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = orderHeaderDto
        };

        _orderServiceMock.Setup(x => x.GetOrderByUserId(It.IsAny<int>()))
                            .ReturnsAsync(expectedResponse);

        // Act
        OrderController orderController = new OrderController(_orderServiceMock.Object, _messageBusMock.Object, _configurationMock.Object);
        var actualResponse = await orderController.Get(orderHeaderDto.UserId);
        var responseContent = actualResponse.Result as OkObjectResult;

        // Assert
        _orderServiceMock.Verify(x => x.GetOrderByUserId(It.IsAny<int>()), Times.Once);
        Assert.NotNull(responseContent);
    }

    [Fact]
    public async void Get_Should_ReturnNotFoundAsSpecifiedUserIdOrderNotExists()
    {
        // Arrange
        Fixture fixture = new Fixture();
        OrderHeaderDto orderHeaderDto = fixture.Create<OrderHeaderDto>();
        ResponseDto<OrderHeaderDto> expectedResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = null,
            IsSuccess = false
        };

        _orderServiceMock.Setup(x => x.GetOrderByUserId(It.IsAny<int>()))
                            .ReturnsAsync(expectedResponse);

        // Act
        OrderController orderController = new OrderController(_orderServiceMock.Object, _messageBusMock.Object, _configurationMock.Object);
        var actualResponse = await orderController.Get(orderHeaderDto.UserId);
        var responseContent = actualResponse.Result as NotFoundObjectResult;

        // Assert
        _orderServiceMock.Verify(x => x.GetOrderByUserId(It.IsAny<int>()), Times.Once);
        Assert.Null(responseContent);
    }

    [Fact]
    public async void CreateOrder_Should_CreateOrderSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        CartDto cartDto = fixture.Create<CartDto>();
        OrderHeaderDto orderHeaderDto = fixture.Create<OrderHeaderDto>();
        ResponseDto<OrderHeaderDto> expectedResponse = new ResponseDto<OrderHeaderDto>()
        {
            Result = orderHeaderDto
        };

        _orderServiceMock.Setup(x => x.CreateOrder(It.IsAny<CartDto>()))
                            .ReturnsAsync(expectedResponse);

        // Act
        OrderController orderController = new OrderController(_orderServiceMock.Object, _messageBusMock.Object, _configurationMock.Object);
        var actualResponse = await orderController.CreateOrder(cartDto);
        var responseContent = actualResponse.Result as OkObjectResult;

        // Assert
        _orderServiceMock.Verify(x => x.CreateOrder(It.IsAny<CartDto>()), Times.Once);
        Assert.NotNull(responseContent);
    }

    [Fact]
    public async void GenerateSlip_Should_GenerateSlipSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        ShippingInfoDto shippingInfoDto = fixture.Create<ShippingInfoDto>();
        ResponseDto<string> expectedResponse = new ResponseDto<string>()
        {
            Result = "Shipping slip generated successfully"
        };

        _messageBusMock.Setup(x => x.PublishMessage(It.IsAny<object>(), It.IsAny<string>()))
                            .ReturnsAsync(expectedResponse);

        var _configurationSection = new Mock<IConfigurationSection>();
        _configurationSection.Setup(x => x.Value).Returns("generateshippingslip");
        _configurationMock.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);
        _configurationMock.Setup(a => a.GetSection("MyKey")).Returns(_configurationSection.Object);

        // Act
        OrderController orderController = new OrderController(_orderServiceMock.Object, _messageBusMock.Object, _configurationMock.Object);
        var actualResponse = await orderController.GenerateShippingSlip(shippingInfoDto);
        var responseContent = actualResponse.Value.Result;

        // Assert
        _messageBusMock.Verify(x => x.PublishMessage(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        Assert.Equal(expectedResponse.Result, responseContent);
    }
}