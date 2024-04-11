using AutoMapper;
using Grpc.Core;
using WarehouseGrpc;
using WarehouseService.Services;

namespace WarehouseService.GrpcServices;

public class WarehouseGrpcService: WarehouseGrpc.WarehouseGrpcService.WarehouseGrpcServiceBase
{
    private readonly IWarehouseService _service;
    private readonly IMapper _mapper;

    public WarehouseGrpcService(IWarehouseService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public override async Task<GetWarehouseListResponse> GetWarehouseList(GetWarehouseListRequest request, ServerCallContext context)
    {
        var result = _service.GetList();
        var listGrpc = _mapper.Map<List<WarehouseGrpc.WarehouseModel>>(result);
        
        var response = new GetWarehouseListResponse()
        {
            Warehouses = { listGrpc }
        };
        await Task.CompletedTask;
        
        return response;

    }

    public override Task<GetWarehouseByIdResponse> GetWarehouseById(GetWarehouseByIdRequest request, ServerCallContext context)
    {
        return base.GetWarehouseById(request, context);
    }

    public override async Task<GetWarehouseByCoordinateResponse> GetWarehouseByCoordinate(GetWarehouseByCoordinateRequest request, ServerCallContext context)
    {
        var point = _mapper.Map<Models.Point>(request.Point);
       
        var result = _service.GetWarehouseByCoordinate(point);
        
        var resultGrpc = _mapper.Map<WarehouseGrpc.WarehouseModel>(result);
        var response = new GetWarehouseByCoordinateResponse()
        {
            Warehouse = resultGrpc
        };
        await Task.CompletedTask;
        return response;
    }
    
    public override async Task<CreateWarehouseResponse> CreateWarehouse(CreateWarehouseRequest request, ServerCallContext context)
    {
        var warehouseLocal = _mapper.Map<Models.Warehouse>(request.Warehouse);
        
        var result = _service.CreateWarehouse(warehouseLocal);
        await Task.CompletedTask;

        return _mapper.Map<CreateWarehouseResponse>(result);
    }
}