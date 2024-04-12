using Moq;
using WarehouseService.Models;
using WarehouseService.Repositories;

namespace WarehouseServiceTests.UnitTests;

public class WarehouseRepositoryUnitTest: IClassFixture<WarehouseMemoryRepository>
{
    private readonly WarehouseMemoryRepository _repository;

    public WarehouseRepositoryUnitTest(WarehouseMemoryRepository repository)
    {
        _repository = repository;
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
        _repository.Insert(warehouse);// Вот тут минус
    }
    
    [Fact]
    public void InsertTest()
    {
        var warehouse = new Warehouse()
        {
            WarehouseId = 2,
            WarehouseName = "TestWarehouse2",
            IsWarehouseClosed = false,
            WarehouseLocationCoordinate = new Point()
            {
                X = 2,
                Y = 2
            },
            WarehouseWorkDays = new List<DayOfWeek>()
            {
                DayOfWeek.Sunday
            }
        };
        _repository.Insert(warehouse);
        var response  = _repository.GetList();
        Assert.NotNull(response);
        Assert.True(response.Exists(item => item.WarehouseId == warehouse.WarehouseId),
            "Результат не содержит добавленного склада");
    }


    [Fact]
    public void GetListTestCount()
    {
        var repositoryMock = new Mock<IWarehouseRepository>();
        repositoryMock
            .Setup(warehouseRepository => warehouseRepository.GetList())
            .Returns(
                new List<Warehouse>()
                {
                    new Warehouse()
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
                    }
                }
            );
        var repository = repositoryMock.Object;
        
        var response  = repository.GetList();
        Assert.NotNull(response);
        Assert.True(response.Count == 1, "Колличество складов не равно 1");
    }
    
    [Fact]
    
    public void GetListTest()
    {
        var response  = _repository.GetList();
        Assert.NotNull(response);
        Assert.True(response.Count > 0, "Колличество складов не больше 0");
 }
    
  
    
    [Fact]
    public void UpdateTest()
    {
        var warehouse = _repository.GetById(1);
        warehouse.WarehouseName = "new name";
        _repository.Update(warehouse);
        var newWarehouse = _repository.GetById(1);
        Assert.True(newWarehouse.WarehouseName == "new name");
    }
    
    [Fact]
    public void DeleteTest()
    {
        Assert.True(false, "Нет теста");
    }
    
    [Fact]
    public void DeleteByIdTest()
    {
        Assert.True(false, "Нет теста");
    }
    
    [Fact]
    public void GetByIdTest()
    {
        Assert.True(false, "Нет теста");
    }
    
    [Fact]
    public void ExistTest()
    {
        Assert.True(false, "Нет теста");
    }
}