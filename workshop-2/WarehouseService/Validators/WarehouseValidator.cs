using FluentValidation;
using WarehouseService.Models;

namespace WarehouseService.Validators;

public class WarehouseValidator: AbstractValidator<Warehouse>
{
    public WarehouseValidator()
    {
        RuleFor(item => item.WarehouseId)
            .NotNull()
            .WithMessage("WarehouseId обязательное поле");
        RuleFor(item => item.WarehouseName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .WithMessage("WarehouseName не прошел валидацию");;
        RuleFor(item => item.IsWarehouseClosed)
            .NotNull()
            .WithMessage("IsWarehouseClosed не прошел валидацию");
        RuleFor(item => item.WarehouseLocationCoordinate)
            .SetValidator(new PointValidator());
    }
}