using AutoMapper;
using WarehouseGrpc;

namespace WarehouseService.Profiles;

public class WarehouseProfile : Profile
{
    public WarehouseProfile()
    {
        CreateMap<WarehouseGrpc.WarehouseModel, Models.Warehouse>()
            .ForMember(item => item.WarehouseId,
                operation => operation.MapFrom(from => from.WarehouseId))
            .ForMember(item => item.WarehouseLocationCoordinate,
                operation => operation.MapFrom(from => from.Coordinate))
            .ForMember(item => item.WarehouseName,
                operation => operation.MapFrom(from => from.WarehouseName))
            .ForMember(item => item.IsWarehouseClosed,
            operation => operation.Ignore())
                .ReverseMap();

        CreateMap<bool, CreateWarehouseResponse>()
            .ForMember(item => item.Result,
                operation => operation.MapFrom(from => from));

    }
}