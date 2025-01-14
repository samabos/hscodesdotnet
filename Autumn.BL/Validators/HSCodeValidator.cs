using Autumn.Domain.Models;
using FluentValidation;

namespace Autumn.Service.Validators
{
    public class HSCodeValidator : AbstractValidator<HSCode>
    {
        public HSCodeValidator()
        {
            RuleFor(x => x.Id).Length(0, 15).NotEmpty();
            RuleFor(x => x.ParentId).Length(0, 15).NotEmpty();
            RuleFor(x => x.Code).Length(0, 15).NotEmpty();
            RuleFor(x => x.ParentCode).Length(0, 15).NotEmpty();
            RuleFor(x => x.Level).NotEmpty().ScalePrecision(1, 0);
            RuleFor(x => x.Order).NotEmpty().ScalePrecision(1, 0);
            RuleFor(x => x.Description).Length(0, 500).NotEmpty();
            RuleFor(x => x.SelfExplanatory).Length(0, 500).NotEmpty();
        }
    }
}
