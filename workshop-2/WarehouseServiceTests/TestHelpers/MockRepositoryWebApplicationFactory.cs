
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using WarehouseService.Models;
using WarehouseService.Repositories;
using WarehouseService.Services;

namespace WarehouseServiceTests.TestHelpers;

public class MockRepositoryWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            services.Replace(ServiceDescriptor.Scoped<IWarehouseRepository>(_ =>
            {
                var repositoryMock = new Mock<IWarehouseRepository>();
                repositoryMock
                    .Setup(warehouseRepository => warehouseRepository.GetList())
                    .Returns(new List<Warehouse>());
                return repositoryMock.Object;
            }));
        });
        
    }
}
