
using FluentValidation;

namespace RandomApp.ShoppingCartManagement.Application.DataTransferObjects
{
    public class ShoppingCartItemDtoValidator : AbstractValidator<ShoppingCartItemDto>
    {
        public ShoppingCartItemDtoValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product ID must be a positive number.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required").MaximumLength(256).WithMessage("Product name cannot exceed 256 characters.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative number.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
