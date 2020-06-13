using BalanceManagement.Contracts.Dtos.Accounts;
using FluentValidation;

namespace BalanceManagement.Api.Validator
{
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        public AccountValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.Name).NotEmpty().MaximumLength(50);
        }
    }
}
