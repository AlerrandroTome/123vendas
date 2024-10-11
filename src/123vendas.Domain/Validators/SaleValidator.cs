using _123vendas.Domain.DTOs;
using FluentValidation;

namespace _123vendas.api.Validators
{
    public class SaleValidator : AbstractValidator<SaleDto>
    {
        public SaleValidator()
        {
            RuleFor(s => s.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(s => s.CustomerName).NotEmpty().WithMessage("Customer name is required.");
            RuleFor(s => s.BranchId).NotEmpty().WithMessage("Branch ID is required.");
            RuleFor(s => s.BranchName).NotEmpty().WithMessage("Branch name is required.");
            RuleForEach(s => s.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty().WithMessage("Product ID is required.");
                item.RuleFor(i => i.ProductName).NotEmpty().WithMessage("Product name is required.");
                item.RuleFor(i => i.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero.");
                item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
                item.RuleFor(i => i.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");
            });
        }
    }
}
