using BalanceManagement.Contracts.Dtos.Accounts;
using FluentValidation;

namespace BalanceManagement.Api.Validator
{
    public class BalanceTransferValidator : AbstractValidator<BalanceTransferDto>
    {
        public BalanceTransferValidator()
        {
            RuleFor(r => r.AccountId).NotEmpty();
            RuleFor(r => r.Amount).NotEmpty().GreaterThan(0);
            RuleFor(r => r.UserTarget).NotEmpty().MaximumLength(50);
            RuleFor(r => r.AccountIdTarget).NotEmpty();

        }
    }
}
