using AutoMapper;
using WarehouseService.Models;

namespace WarehouseService.Profiles;

public class PointProfile : Profile
{
    public PointProfile()
    {
        CreateMap<WarehouseGrpc.PointModel, Models.Point>()
            .ForMember(item => item.X,
                operation => operation.MapFrom(from => from.X))
            .ForMember(item => item.Y,
                operation => operation.MapFrom(from => from.Y))
            .ReverseMap();
    }
}