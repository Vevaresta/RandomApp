
using FluentValidation;

namespace RandomApp.ShoppingCartManagement.Application.DataTransferObjects
{
    public class ShoppingCartDtoValidator : AbstractValidator<ShoppingCartDto>
    {
        public ShoppingCartDtoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID must be a positive number.");
            RuleFor(x => x.CreatedAt).NotEmpty().WithMessage("Creation date is required.");
            RuleFor(x => x.Items).NotNull().WithMessage("Items collection cannot be null.");
            RuleForEach(x => x.Items).SetValidator(new ShoppingCartItemDtoValidator());
        }
    }
}
