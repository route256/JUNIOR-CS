using Moq;
using WarehouseService.Models;
using WarehouseService.Repositories;
using WarehouseService = WarehouseService.Services.WarehouseService;

namespace WarehouseServiceTests.UnitTests;

public class WarehouseServiceUnitTest
{
    [Fact]
    public void CreateWarehouseTest()
    {
        var warehouse = new Warehouse()
        {
            WarehouseId = 1,
            WarehouseName = "TestWarehouse",
            IsWarehouseClosed = false,
            WarehouseLocationCoordinate = new Point()
            {
                X = 1,
                Y = 1
            },
            WarehouseWorkDays = new List<DayOfWeek>()
            {
                DayOfWeek.Sunday
            }
        };
        
        var repositoryMock = new Mock<IWarehouseRepository>();
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetById(It.IsAny<long>()))
            .Returns(warehouse);
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetList())
            .Returns(new List<Warehouse>(){warehouse});
        var repository = repositoryMock.Object;

        var service = new global::WarehouseService.Services.WarehouseService(repository);
        service.CreateWarehouse(warehouse);
        var list = service.GetList();
        Assert.True(list.Exists(item => item.WarehouseId == warehouse.WarehouseId));
    }
    
    [Fact]
    public void DeleteWarehouseTest()
    {
        Assert.True(false, "Нет теста");
    }
    
    [Fact]
    public void DeleteWarehouseByIdTest()
    {
        var warehouse = new Warehouse()
        {
            WarehouseId = 1,
            WarehouseName = "TestWarehouse",
            IsWarehouseClosed = false,
            WarehouseLocationCoordinate = new Point()
            {
                X = 1,
                Y = 1
            },
            WarehouseWorkDays = new List<DayOfWeek>()
            {
                DayOfWeek.Sunday
            }
        };
        
        var repositoryMock = new Mock<IWarehouseRepository>();
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetById(It.IsAny<long>()))
            .Returns(warehouse);
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetList())
            .Returns(new List<Warehouse>(){warehouse});
        var repository = repositoryMock.Object;

        var service = new global::WarehouseService.Services.WarehouseService(repository);
        var list = service.GetList();
        Assert.True(list.Exists(item => item.WarehouseId == warehouse.WarehouseId));
        service.DeleteWarehouseById(warehouse.WarehouseId);
        
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetList())
            .Returns(new List<Warehouse>());
        
        var listAfterDelete = service.GetList();
        Assert.False(listAfterDelete.Exists(item => item.WarehouseId == warehouse.WarehouseId));
 }
    
    [Fact]
    public void GetListTest()
    {
        Assert.True(false, "Нет теста");
    }
    
    [Fact]
    public void UpdateWorkDayTest()
    {
        Assert.True(false, "Нет теста");
    }
    
    [Theory]
    [MemberData("GetWarehouseByCoordinateTestData")]
    public void GetWarehouseByCoordinateTest(
        Point warehouseOnePoint,
        Point warehouseTwoPoint, 
        Point point, 
        bool isWarehouseOne)
    {
        var warehouse1 = new Warehouse()
        {
            WarehouseId = 1,
            WarehouseLocationCoordinate = warehouseOnePoint,
        };
        
        var warehouse2 = new Warehouse()
        {
            WarehouseId = 2,
            WarehouseLocationCoordinate = warehouseTwoPoint
        };
        
        var repositoryMock = new Mock<IWarehouseRepository>();
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetList())
            .Returns(new List<Warehouse>(){warehouse1, warehouse2});
       
        var service = new global::WarehouseService.Services.WarehouseService(repositoryMock.Object);
        var warehouse = service.GetWarehouseByCoordinate(point);
        Assert.Equal(isWarehouseOne ? warehouse1 : warehouse2, warehouse);
    }

    public static IEnumerable<object[]> GetWarehouseByCoordinateTestData()
    {
        yield return new object[]
        {
            new Point() { X = 30, Y = 30 },
            new Point() { X = 40, Y = 40 },
            new Point() { X = 30, Y = 30 },
            true
        };
        yield return new object[]
        {
            new Point() { X = 0, Y = 0 },
            new Point() { X = 100, Y = 100 },
            new Point() { X = 30, Y = 30 },
            true
        };
        yield return new object[]
        {
            new Point() { X = -200, Y = -200 },
            new Point() { X = 100, Y = 100 },
            new Point() { X = 30, Y = 30 },
            false
        };
        yield return new object[] // Не понятно что должно возвращаться
        {
            new Point() { X = 10, Y = 10 },
            new Point() { X = 10, Y = 10 },
            new Point() { X = 10, Y = 10 },
            true
        };
    }
}