using WarehouseService.Models;
using WarehouseService.Repositories;

namespace WarehouseService.Services;

public class WarehouseService: IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }


    public bool CreateWarehouse(Warehouse warehouse)
    {
        _warehouseRepository.Insert(warehouse);
        return true;
    }

    public bool DeleteWarehouse(Warehouse warehouse)
    {
        _warehouseRepository.Delete(warehouse);
        return true;
    }

    public bool DeleteWarehouseById(long id)
    {
        _warehouseRepository.DeleteById(id);
        return true;
    }

    public List<Warehouse> GetList()
    {
        return (List<Warehouse>)_warehouseRepository.GetList();

    }

    public bool UpdateWorkDay(long id, List<DayOfWeek> newWorkDay)
    {
        var warehouse = _warehouseRepository.GetById(id);
        warehouse.WarehouseWorkDays = newWorkDay;
        _warehouseRepository.Update(warehouse);
        return true;
    }

    public Warehouse GetWarehouseByCoordinate(Point point)
    {
        var warehouses = _warehouseRepository.GetList();
        var warehouse = warehouses.MinBy(item => GetSquareLine(item.WarehouseLocationCoordinate, point));
        return warehouse;
    }

    private double GetSquareLine(Point pointStart, Point pointEnd)
    {
        return (pointStart.X - pointEnd.X) * (pointStart.X - pointEnd.X) +
               (pointStart.Y - pointEnd.Y) * (pointStart.Y - pointEnd.Y);
    }
}