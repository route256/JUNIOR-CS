using AutoMapper;
using FluentAssertions;
using WarehouseGrpc;
using WarehouseService.Models;
using WarehouseService.Profiles;
using WarehouseServiceTests.TestHelpers;

namespace WarehouseServiceTests.UnitTests;

public class PointMappingUnitTest
{
    [Theory]
    [MemberData("PointMappingUnitTestMapData")]
    public void PointMappingUnitTestMap(WarehouseGrpc.PointModel pointModel)
    {
        var mapper = MapperHelper.GetMapper();

        var point = mapper.Map<Point>(pointModel);

        point.Should().NotBeNull();
        point.X.Should().Be(pointModel.X);
        point.Y.Should().Be(pointModel.Y);
    }

    public static IEnumerable<object[]> PointMappingUnitTestMapData()
    {
        yield return new object[] { new PointModel() { X = 100, Y = 100 } };
        yield return new object[] { new PointModel() { X = -100, Y = -100 } };
        yield return new object[] { new PointModel() { X = 0, Y = 0 } };
        yield return new object[] { new PointModel() { X = -50, Y = 50 } };
    }
    
    [Fact]
    public void PointMappingUnitTestMapNull()
    {
        var configuration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<PointProfile>();
        });
        var mapper = new Mapper(configuration);

        var point = mapper.Map<Point>(null);
        point.Should().BeNull();
    }
}