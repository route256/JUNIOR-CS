using FluentAssertions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using WarehouseGrpc;
using WarehouseService.Models;
using WarehouseService.Repositories;
using WarehouseService.Services;

namespace WarehouseServiceTests.IntegrationTests;

public class WarehouseGrpcServiceIntegrationTest:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WarehouseGrpcServiceIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [MemberData("IntegrationTestGetListData")]
    public void IntegrationTestGetList(List<Warehouse> warehouses)
    {
        var factory =_factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Scoped<IWarehouseService>(_ =>
                {
                    var repositoryMock = new Mock<IWarehouseService>();
                    repositoryMock
                        .Setup(warehouseRepository => warehouseRepository.GetList())
                        .Returns(warehouses);
                    return repositoryMock.Object;
                }));
            });
        });
        var clientWebApp = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });
        var grpcClient = new WarehouseGrpcService.WarehouseGrpcServiceClient(channel);
        var response = grpcClient.GetWarehouseList(new GetWarehouseListRequest());

        response.Should().NotBeNull();
        response.Warehouses.Should().NotBeNull();
        response.Warehouses.Count.Should().Be(warehouses.Count);
        response.Warehouses.Select(item => item.WarehouseId)
            .Should()
            .Match(item =>  item.Except(warehouses.Select(i => i.WarehouseId)).Count() == 0);
    }
    
    
    [Theory]
    [MemberData("IntegrationTestGetListData")]
    public void IntegrationTestGetListMockRepository(List<Warehouse> warehouses)
    {
        var factory =_factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton<IWarehouseRepository>(_ =>
                {
                    var repositoryMock = new Mock<IWarehouseRepository>();
                    repositoryMock
                        .Setup(warehouseRepository => warehouseRepository.GetList())
                        .Returns(warehouses);
                    return repositoryMock.Object;
                }));
            });
        });
        var clientWebApp = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });
        var grpcClient = new WarehouseGrpcService.WarehouseGrpcServiceClient(channel);
        var response = grpcClient.GetWarehouseList(new GetWarehouseListRequest());

        response.Should().NotBeNull();
        response.Warehouses.Should().NotBeNull();
        response.Warehouses.Count.Should().Be(warehouses.Count);
        response.Warehouses.Select(item => item.WarehouseId)
            .Should()
            .Match(item =>  item.Except(warehouses.Select(i => i.WarehouseId)).Count() == 0);
    }

    public static IEnumerable<object[]> IntegrationTestGetListData()
    {
        yield return new object[] { new List<Warehouse>()
        {
                new()
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
                },
                new()
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
                        DayOfWeek.Monday
                    }
                }
        } };
    }
    
    
    [Theory]
    [MemberData("IntegrationTestGetByCoordinateData")]
    public void IntegrationTestGetByCoordinate(List<Warehouse> warehouses,GetWarehouseByCoordinateRequest request, long warehouseId)
    {
        var factory =_factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton<IWarehouseRepository>(_ =>
                {
                    var repositoryMock = new Mock<IWarehouseRepository>();
                    repositoryMock
                        .Setup(warehouseRepository => warehouseRepository.GetList())
                        .Returns(warehouses);
                    return repositoryMock.Object;
                }));
            });
        });
        var clientWebApp = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });
        var grpcClient = new WarehouseGrpcService.WarehouseGrpcServiceClient(channel);
        var response = grpcClient.GetWarehouseByCoordinate(request);

        response.Should().NotBeNull();
        response.Warehouse.Should().NotBeNull();
        response.Warehouse.WarehouseId.Should().Be(warehouseId);
    }
    
    public static IEnumerable<object[]> IntegrationTestGetByCoordinateData()
    {
        yield return new object[] { new List<Warehouse>()
        {
            new()
            {
                WarehouseId = 1,
                WarehouseName = "TestWarehouse",
                IsWarehouseClosed = false,
                WarehouseLocationCoordinate = new Point()
                {
                    X = 10,
                    Y = 10
                },
                WarehouseWorkDays = new List<DayOfWeek>()
                {
                    DayOfWeek.Sunday
                }
            },
            new()
            {
                WarehouseId = 2,
                WarehouseName = "TestWarehouse2",
                IsWarehouseClosed = false,
                WarehouseLocationCoordinate = new Point()
                {
                    X = 70,
                    Y = 70
                },
                WarehouseWorkDays = new List<DayOfWeek>()
                {
                    DayOfWeek.Monday
                }
            }
        } ,
            new GetWarehouseByCoordinateRequest()
            {
                Point = new PointModel()
                {
                    X = 30,
                    Y = 30
                }
            },
            1
        };
    }
}