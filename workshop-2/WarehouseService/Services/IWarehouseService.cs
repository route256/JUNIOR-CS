using WarehouseService.Models;

namespace WarehouseService.Services;

public interface IWarehouseService
{
    bool CreateWarehouse(Warehouse warehouse);
    bool DeleteWarehouse(Warehouse warehouse);
    bool DeleteWarehouseById(long id);
    List<Warehouse> GetList();
    bool UpdateWorkDay(long id, List<DayOfWeek> newWorkDay);
    Warehouse GetWarehouseByCoordinate(Point point);
}