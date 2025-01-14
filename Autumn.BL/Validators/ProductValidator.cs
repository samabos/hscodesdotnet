using Autumn.Domain.Models;
using FluentValidation;

namespace Autumn.Service.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() {
            RuleFor(x => x.Class).Length(0, 10).NotEmpty();
            RuleFor(x => x.Code).Length(0,13).NotEmpty();
            RuleFor(x => x.Keyword).Length(0,50).NotEmpty();
        }
    }
}
