using FluentValidation;
using WarehouseService.Models;

namespace WarehouseService.Validators;

public class PointValidator: AbstractValidator<Point>
{
    public PointValidator()
    {
        RuleFor(item => item.X)
            .NotNull()
            .Must(item => item > 0)
            .WithMessage("X не прошел валидацию");
        RuleFor(item => item.Y)
            .NotEmpty()
            .Must(item => item > 0)
            .WithMessage("Y не прошел валидацию");
    }
}