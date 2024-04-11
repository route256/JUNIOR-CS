using AutoMapper;
using FluentAssertions;
using Grpc.Net.Client;
using WarehouseGrpc;
using WarehouseService.Models;
using WarehouseService.Profiles;
using WarehouseServiceTests.TestHelpers;

namespace WarehouseServiceTests.IntegrationTests;

public class IntegrationTest: IClassFixture<MockServiceWebApplicationFactory<Program>>
{
    private readonly MockServiceWebApplicationFactory<Program> _factory;

    public IntegrationTest(MockServiceWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void IntegrationTestGetList()
    {
        var clientWebApp = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });
        var grpcClient = new WarehouseGrpcService.WarehouseGrpcServiceClient(channel);
        var response = grpcClient.GetWarehouseList(new GetWarehouseListRequest());

        response.Should().NotBeNull();
        response.Warehouses.Should().NotBeNull();
        response.Warehouses.Count.Should().Be(1);
    }
}