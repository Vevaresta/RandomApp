using FluentValidation;

namespace RandomApp.ProductManagement.Application.DataTransferObjects
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Price must be positive");
            RuleFor(x => x.Currency).NotEmpty().Length(3).WithMessage("Valid currency code is required");
            RuleFor(x => x.SKU).NotEmpty().MaximumLength(15);
            RuleFor(x => x.ProductDescription).NotEmpty().MaximumLength(1000);
        }
    }
}
