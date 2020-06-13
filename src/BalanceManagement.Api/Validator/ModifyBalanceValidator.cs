using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos.Accounts;
using FluentValidation;

namespace BalanceManagement.Api.Validator
{
    public class ModifyBalanceValidator : AbstractValidator<ModifyBalanceDto>
    {
        public ModifyBalanceValidator()
        {
            RuleFor(r => r.AccountId).NotEmpty();
            RuleFor(r => r.Description).NotEmpty().MaximumLength(250);
            RuleFor(r => r.Amount).NotEmpty().GreaterThan(0); 

        }
    }
}
