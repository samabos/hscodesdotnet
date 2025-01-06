using Autumn.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.Domain.Validators
{
    public class CustomsTariffValidator : AbstractValidator<CustomsTariff>
    {
        public CustomsTariffValidator() {
            RuleFor(x => x.Header).Length(0, 7).NotEmpty();
            RuleFor(x => x.HSCode).Length(0,13).NotEmpty();
            RuleFor(x => x.Description).Length(0,500).NotEmpty();
            RuleFor(x => x.DUTY).ScalePrecision(2, 2).NotEmpty();
            RuleFor(x => x.VAT).ScalePrecision(2, 2).NotEmpty();
            RuleFor(x => x.LEVY).ScalePrecision(2, 2).NotEmpty();
            RuleFor(x => x.NAC).ScalePrecision(2, 2).NotEmpty();
            RuleFor(x => x.SUR).ScalePrecision(2, 2).NotEmpty();
            RuleFor(x => x.ETLS).ScalePrecision(2, 2).NotEmpty();
            RuleFor(x => x.CISS).ScalePrecision(2, 2).NotEmpty();
        }
    }
}
