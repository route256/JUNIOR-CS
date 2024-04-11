using System.Dynamic;
using WarehouseService.Models;

namespace WarehouseService.Repositories;

public interface IWarehouseRepository
{
    List<Warehouse> GetList();
    bool Insert(Warehouse warehouse);
    void Update(Warehouse warehouse);
    void Delete(Warehouse warehouse);
    void DeleteById(long id);
    Warehouse GetById(long warehouseId);
    bool Exist(long id);
}