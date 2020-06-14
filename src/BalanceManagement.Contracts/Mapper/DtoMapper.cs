using AutoMapper;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Data.Entities;

namespace BalanceManagement.Contracts.Mapper
{
    /// <summary>
    ///     Configuration of Dtos
    /// </summary>
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<UserDto, User>().ForMember(f => f.Password, map => map.Ignore()); ;
            CreateMap<User, UserWithBalanceDto>().ForMember(f => f.Password, map => map.Ignore());
            CreateMap<User, UserDto>()
                .ForMember(f => f.Password, map => map.Ignore());
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>();
            //CreateMap<Account, AccountDetailsDto>()
            //    .ForMember(f=>f.AccountBalances, map=>map.MapFrom(m=>m.AccountBalances));
            CreateMap<AccountTransaction, AccountTransactionsDto>();
            CreateMap<ModifyBalanceDto, AccountTransaction>();
            ;
        }
    }
}