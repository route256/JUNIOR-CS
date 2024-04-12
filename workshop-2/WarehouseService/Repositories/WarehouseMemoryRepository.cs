using WarehouseService.Models;

namespace WarehouseService.Repositories;

public class WarehouseMemoryRepository: IWarehouseRepository
{
    private readonly Dictionary<long,Warehouse> _warehousesDictionary;

    public WarehouseMemoryRepository()
    {
        _warehousesDictionary = new Dictionary<long, Warehouse>();
    }


    public List<Warehouse> GetList()
    {
        return _warehousesDictionary.Values.ToList();
    }

    public bool Insert(Warehouse warehouse)
    {
        return _warehousesDictionary.TryAdd(warehouse.WarehouseId,warehouse);
    }

    public void Update(Warehouse warehouse)
    {
        _warehousesDictionary[warehouse.WarehouseId] = warehouse;
    }

    public void Delete(Warehouse warehouse)
    {
        _warehousesDictionary.Remove(warehouse.WarehouseId);
    }

    public void DeleteById(long id)
    {
        _warehousesDictionary.Remove(id);
    }

    public Warehouse GetById(long warehouseId)
    {
        return _warehousesDictionary[warehouseId];
    }

    public bool Exist(long id)
    {
        return _warehousesDictionary.ContainsKey(id);
    }
}