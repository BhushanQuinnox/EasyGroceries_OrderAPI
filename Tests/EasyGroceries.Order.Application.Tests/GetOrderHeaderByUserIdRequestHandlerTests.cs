using AutoMapper;
using EasyGroceries.Order.Application.Contracts.Infrastructure;
using EasyGroceries.Order.Application.Features.OrderHeader.Handlers.Queries;
using EasyGroceries.Order.Application.Features.OrderHeader.Requests.Queries;
using EasyGroceries.Order.Application.Profiles;
using EasyGroceries.Order.Domain;
using Moq;

namespace EasyGroceries.Order.Application.Tests;

public class GetOrderHeaderByUserIdRequestHandlerTests
{
    private readonly Mock<IOrderHeaderRepository> _orderHeaderRepositoryMock;
    private List<OrderHeader> _orderHeaderLst;

    public GetOrderHeaderByUserIdRequestHandlerTests()
    {
        _orderHeaderRepositoryMock = new Mock<IOrderHeaderRepository>();
    }

    private void IntializeOrderHeaderData()
    {
        _orderHeaderLst = new List<OrderHeader>()
        {
            new OrderHeader(){OrderHeaderId = 1, OrderTotal = 100, LoyaltyMembershipOpted = false, UserId = 1000, OrderStatus = OrderStatusEnum.Pending},
            new OrderHeader(){OrderHeaderId = 2, OrderTotal = 101, LoyaltyMembershipOpted = true, UserId = 2000, OrderStatus = OrderStatusEnum.Processed},
            new OrderHeader(){OrderHeaderId = 3, OrderTotal = 102, LoyaltyMembershipOpted = true, UserId = 3000, OrderStatus = OrderStatusEnum.Completed},
            new OrderHeader(){OrderHeaderId = 4, OrderTotal = 103, LoyaltyMembershipOpted = false, UserId = 4000, OrderStatus = OrderStatusEnum.Pending}
        };
    }

    [Fact]
    public async Task Handle_Should_ReturnsOrderHeaderByUserId()
    {
        // Arrange
        IntializeOrderHeaderData();
        int userId = 2000;
        var command = new GetOrderHeaderByUserIdRequest() { UserId = userId };
        var orderHeader = _orderHeaderLst.FirstOrDefault(x => x.UserId == userId);

        _orderHeaderRepositoryMock.Setup(x => x.GetOrderHeaderByUserId(userId))
                .ReturnsAsync(orderHeader);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new GetOrderHeaderByUserIdRequestHandler(_orderHeaderRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.NotNull(result);
        result.OrderTotal.Equals(101);
        Assert.True(result.LoyaltyMembershipOpted);
        result.OrderStatus.Equals(OrderStatusEnum.Processed);
    }
}