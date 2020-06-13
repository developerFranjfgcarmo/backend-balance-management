using BalanceManagement.Contracts.Dtos.Users;
using FluentValidation;

namespace BalanceManagement.Api.Validator
{
    public class AuthenticateValidator : AbstractValidator<AuthenticateDto>
    {
        public AuthenticateValidator()
        {
            RuleFor(r => r.Username).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
        }
    }
}
