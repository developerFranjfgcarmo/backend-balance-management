using BalanceManagement.Contracts.Dtos.Users;
using FluentValidation;

namespace BalanceManagement.Api.Validator
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(r => r.UserName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Password).NotEmpty().MaximumLength(250);
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Surname).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Nick).NotEmpty().MaximumLength(50);
            RuleFor(r => r.RoleId).NotEmpty();
        }
    }
}
