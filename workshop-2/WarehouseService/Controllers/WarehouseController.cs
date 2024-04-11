using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WarehouseService.Models;
using WarehouseService.Services;

namespace WarehouseService.Controllers;

[ApiController]
[Route("[controller]")]
public class WarehouseController : ControllerBase
{
   private readonly IWarehouseService _service;
   private readonly IValidator<Warehouse> _validator;

   public WarehouseController(
      IWarehouseService service, 
      IValidator<Warehouse> validator)
   {
      _service = service;
      _validator = validator;
   }

   [HttpPost("[action]")]
   public bool CreateWarehouse(Warehouse warehouse)
   {
      var validationResult = _validator.Validate(warehouse);
      if (!validationResult.IsValid)
         return false;
      
      return _service.CreateWarehouse(warehouse);
   }
   
   [HttpGet("[action]")]
   public List<Warehouse> GetWarehouseList()
   {
      return _service.GetList();
   }
   
   [HttpPost("[action]")]
   public bool DeleteWarehouseById(long id)
   {
      return _service.DeleteWarehouseById(id);
   }
   
   [HttpPost("[action]")]
   public Warehouse GetWarehouseByCoordinate(Point point)
   {
      return _service.GetWarehouseByCoordinate(point);
   }
}