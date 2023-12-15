

using System.Data;
using AutoFixture;
using Dapper;
using EasyGroceries.Order.Domain;
using EasyGroceries.Order.Infrastructure.Contracts;
using EasyGroceries.Order.Infrastructure.Repositories;
using Moq;

namespace EasyGroceries.Order.Infrastructure.Tests;

public class OrderDetailsRepositoryTests
{
    private readonly Mock<IDapper> _dapperMock;

    public OrderDetailsRepositoryTests()
    {
        _dapperMock = new Mock<IDapper>();
    }

    [Fact]
    public void Add_Should_InsertsOrderDetailsRecordInDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var orderDetails = fixture.Create<OrderDetails>();
        List<OrderDetails> orderDetailsLst = new List<OrderDetails>();
        _dapperMock.Setup(x => x.Insert(It.IsAny<string>(), orderDetails, It.IsAny<CommandType>()))
                        .Callback(() =>
                        {
                            orderDetailsLst.Add(orderDetails);
                        });
        // Act
        OrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository(_dapperMock.Object);
        var result = orderDetailsRepository.Add(orderDetails);

        // Assert
        _dapperMock.Verify(x => x.Insert(It.IsAny<string>(), orderDetails, It.IsAny<CommandType>()),
              Times.Once);
        Assert.Equal(1, orderDetailsLst.Count);
        Assert.Equal(orderDetails.OrderHeaderId, orderDetailsLst.FirstOrDefault().OrderHeaderId);
        Assert.Equal(orderDetails.OrderDetailsId, orderDetailsLst.FirstOrDefault().OrderDetailsId);
        Assert.Equal(orderDetails.Count, orderDetailsLst.FirstOrDefault().Count);
        Assert.Equal(orderDetails.ProductId, orderDetailsLst.FirstOrDefault().ProductId);
        Assert.Equal(orderDetails.Price, orderDetailsLst.FirstOrDefault().Price);
    }

    [Fact]
    public void GetAllOrderDetails_Should_ReturnAllOrderDetailsRecordPresentInDB()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var orderDetailsLst = fixture.Create<List<OrderDetails>>();
        _dapperMock.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<OrderDetails>(), It.IsAny<CommandType>()))
                        .Returns(orderDetailsLst);
        // Act
        OrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository(_dapperMock.Object);
        var result = orderDetailsRepository.GetAllOrderDetails();

        // Assert
        Assert.Equal(orderDetailsLst.Count, result.Result.Count);
        Assert.Equal(orderDetailsLst.FirstOrDefault().OrderHeaderId, result.Result.FirstOrDefault().OrderHeaderId);
        Assert.Equal(orderDetailsLst.FirstOrDefault().OrderDetailsId, result.Result.FirstOrDefault().OrderDetailsId);
        Assert.Equal(orderDetailsLst.FirstOrDefault().Count, result.Result.FirstOrDefault().Count);
        Assert.Equal(orderDetailsLst.FirstOrDefault().ProductId, result.Result.FirstOrDefault().ProductId);
        Assert.Equal(orderDetailsLst.FirstOrDefault().ProductName, result.Result.FirstOrDefault().ProductName);
        Assert.Equal(orderDetailsLst.FirstOrDefault().Price, result.Result.FirstOrDefault().Price);
    }

    [Fact]
    public void GetOrderDetailsByOrderHeaderId_Should_Return_OrderDetailsOfSpecifiedHeaderId()
    {
        // Arrange
        var fixture = new Fixture();
        var orderDetailsList = fixture.Create<IReadOnlyList<OrderDetails>>();
        int orderHeaderId = orderDetailsList[1].OrderHeaderId;
        var expectedOrderDetails = orderDetailsList.Where(x => x.OrderHeaderId == orderHeaderId);
        _dapperMock.Setup(x => x.GetAll<OrderDetails>(It.IsAny<string>(),
                 It.IsAny<OrderDetails>(), It.IsAny<CommandType>())).Returns(expectedOrderDetails.ToList());

        // Act
        OrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository(_dapperMock.Object);
        var result = orderDetailsRepository.GetOrderDetailsByOrderHeaderId(orderHeaderId);

        // Assert
        _dapperMock.Verify(x => x.GetAll<OrderDetails>(It.IsAny<string>(),
                 It.IsAny<OrderDetails>(), It.IsAny<CommandType>()), Times.Once);
        Assert.Equal(expectedOrderDetails.Count(), result.Result.Count);
    }

    [Fact]
    public void AddOrderDetailsList_Should_InsertsOrderDetailsListInDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var orderDetailsDummy = fixture.Create<List<OrderDetails>>();
        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        _dapperMock.Setup(x => x.InsertList(It.IsAny<string>(), orderDetailsDummy, It.IsAny<CommandType>()))
                        .Callback(() =>
                        {
                            orderDetailsList.AddRange(orderDetailsDummy);
                        });

        var orderDetailsCount = orderDetailsDummy.Count;

        // Act
        OrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository(_dapperMock.Object);
        var result = orderDetailsRepository.AddOrderDetailsList(orderDetailsDummy);

        // Assert
        _dapperMock.Verify(x => x.InsertList(It.IsAny<string>(), orderDetailsDummy, It.IsAny<CommandType>()),
              Times.Once);
        Assert.Equal(orderDetailsCount, orderDetailsList.Count);
        Assert.Equal(orderDetailsDummy.FirstOrDefault().OrderHeaderId, orderDetailsList.FirstOrDefault().OrderHeaderId);
        Assert.Equal(orderDetailsDummy.FirstOrDefault().OrderDetailsId, orderDetailsList.FirstOrDefault().OrderDetailsId);
        Assert.Equal(orderDetailsDummy.FirstOrDefault().Count, orderDetailsList.FirstOrDefault().Count);
        Assert.Equal(orderDetailsDummy.FirstOrDefault().ProductId, orderDetailsList.FirstOrDefault().ProductId);
        Assert.Equal(orderDetailsDummy.FirstOrDefault().Price, orderDetailsList.FirstOrDefault().Price);
    }

}