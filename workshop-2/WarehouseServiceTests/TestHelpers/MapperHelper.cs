using AutoMapper;
using WarehouseService.Profiles;

namespace WarehouseServiceTests.TestHelpers;

public static class MapperHelper
{
    public static IMapper GetMapper()
    {
        var configuration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<PointProfile>();
            expression.AddProfile<WarehouseProfile>();
        });
        var mapper = new Mapper(configuration);
        return mapper;
    }
}