
using System.Data;
using AutoFixture;
using Dapper;
using EasyGroceries.Order.Domain;
using EasyGroceries.Order.Infrastructure.Contracts;
using EasyGroceries.Order.Infrastructure.Repositories;
using Moq;

namespace EasyGroceries.Order.Infrastructure.Tests;

public class OrderHeaderRepositoryTests
{
    private readonly Mock<IDapper> _dapperMock;

    public OrderHeaderRepositoryTests()
    {
        _dapperMock = new Mock<IDapper>();
    }


    [Fact]
    public void Add_Should_InsertsOrderHeaderRecordInDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var orderHeader = fixture.Create<OrderHeader>();
        List<OrderHeader> orderHeaderLst = new List<OrderHeader>();
        _dapperMock.Setup(x => x.Insert(It.IsAny<string>(), orderHeader, It.IsAny<CommandType>()))
                        .Callback(() =>
                        {
                            orderHeaderLst.Add(orderHeader);
                        });

        var orderHeaderLstCountBeforeAdd = orderHeaderLst.Count;

        // Act
        OrderHeaderRepository orderHeaderRepository = new OrderHeaderRepository(_dapperMock.Object);
        var result = orderHeaderRepository.Add(orderHeader);

        // Assert
        _dapperMock.Verify(x => x.Insert(It.IsAny<string>(), orderHeader, It.IsAny<CommandType>()),
              Times.Once);
        Assert.NotEqual(orderHeaderLstCountBeforeAdd, orderHeaderLst.Count);
        Assert.Equal(orderHeader.OrderHeaderId, orderHeaderLst[0].OrderHeaderId);
        Assert.Equal(orderHeader.UserId, orderHeaderLst[0].UserId);
        Assert.Equal(orderHeader.LoyaltyMembershipOpted, orderHeaderLst[0].LoyaltyMembershipOpted);
        Assert.Equal(orderHeader.OrderTotal, orderHeaderLst[0].OrderTotal);
        Assert.Equal(orderHeader.OrderDetails.Count(), orderHeaderLst[0].OrderDetails.Count());
    }

    [Fact]
    public void GetOrdertHeaderByUserId_Should_Return_OrderHeaderOfSpecifiedUserId()
    {
        // Arrange
        var fixture = new Fixture();
        var orderHeaderList = fixture.Create<IReadOnlyList<OrderHeader>>();
        int orderHeaderIndex = 1;
        var expectedOrderHeader = orderHeaderList[orderHeaderIndex];
        _dapperMock.Setup(x => x.Get<OrderHeader>(It.IsAny<string>(),
                 It.IsAny<DynamicParameters>(), It.IsAny<CommandType>())).Returns(expectedOrderHeader);

        // Act
        OrderHeaderRepository orderHeaderRepository = new OrderHeaderRepository(_dapperMock.Object);
        var result = orderHeaderRepository.GetOrderHeaderByUserId(expectedOrderHeader.UserId);

        // Assert
        Assert.Equal(expectedOrderHeader.OrderHeaderId, result.Result.OrderHeaderId);
        Assert.Equal(expectedOrderHeader.OrderTotal, result.Result.OrderTotal);
        Assert.Equal(expectedOrderHeader.LoyaltyMembershipOpted, result.Result.LoyaltyMembershipOpted);
        Assert.Equal(expectedOrderHeader.OrderTime, result.Result.OrderTime);
        Assert.Equal(expectedOrderHeader.UserId, result.Result.UserId);
    }
}