using AutoMapper;
using FluentAssertions;
using WarehouseGrpc;
using WarehouseService.Models;
using WarehouseService.Profiles;
using WarehouseServiceTests.TestHelpers;

namespace WarehouseServiceTests.UnitTests;

public class WarehouseMappingUnitTest
{
    [Fact]
    public void WarehouseMappingUnitTestMap()
    {
        var configuration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<PointProfile>();
            expression.AddProfile<WarehouseProfile>();
        });
        var mapper = new Mapper(configuration);

        var warehouseGrpc = new WarehouseModel()
        {
            WarehouseId = 1,
            WarehouseName = "test",
            Coordinate = new PointModel()
            {
                X = 100,
                Y = 100
            },
            WorkDays = { WorkDay.Sunday }
        };
        var warehouse = mapper.Map<Warehouse>(warehouseGrpc);

        warehouse.Should().NotBeNull();
        warehouse.WarehouseId.Should().Be(1);
        warehouse.WarehouseName.Should().Be("test");
        warehouse.IsWarehouseClosed.Should().Be(false);
        warehouse.WarehouseLocationCoordinate.Should().NotBeNull();
        warehouse.WarehouseLocationCoordinate.X.Should().Be(100);
        warehouse.WarehouseLocationCoordinate.Y.Should().Be(100);
        warehouse.WarehouseWorkDays
            .Should()
            .Match(item => item.Any(week => week == DayOfWeek.Sunday));
    }
    
    [Fact]
    public void WarehouseMappingUnitTestMapReverse()
    {
        var mapper = MapperHelper.GetMapper();

        var warehouse = new Warehouse()
        {
            WarehouseId = 1,
            WarehouseName = "test",
           IsWarehouseClosed = true,
           WarehouseLocationCoordinate = new Point()
           {
               X = 100,
               Y = 100
           },
           WarehouseWorkDays = new List<DayOfWeek>()
           {
               DayOfWeek.Sunday
           }
        };
        var warehouseGrpc = mapper.Map<WarehouseModel>(warehouse);

        warehouseGrpc.Should().NotBeNull();
        warehouseGrpc.WarehouseId.Should().Be(1);
        warehouseGrpc.WarehouseName.Should().Be("test");
        warehouseGrpc.Coordinate.Should().NotBeNull();
        warehouseGrpc.Coordinate.X.Should().Be(100);
        warehouseGrpc.Coordinate.Y.Should().Be(100);
        warehouseGrpc.WorkDays
            .Should()
            .Match(item => item.Any(week => week == WorkDay.Sunday));
    }
}