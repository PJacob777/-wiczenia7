using Ćwiczenia_7.DTOs;
using FluentValidation;

namespace Ćwiczenia_7.Validators;

public class ProductValidator : AbstractValidator<GetProductResponse>
{
    public ProductValidator()
    {
        RuleFor(e => e.IdProduct).NotNull();
        RuleFor((e => e.Amount)).NotNull();
        RuleFor(e => e.IdWarehouae).NotNull();
        RuleFor(e => e.CreatedAt).NotNull();
    }
}