namespace WarehouseService.Models;

public class Warehouse
{
    // Возможно стоит ограничить set, record
    
    public long WarehouseId { get; set; }
    
    public string WarehouseName { get; set; }
    
    public Point WarehouseLocationCoordinate { get; set; }
    
    public List<DayOfWeek> WarehouseWorkDays { get; set; }
    
    public bool IsWarehouseClosed{ get; set; }
}