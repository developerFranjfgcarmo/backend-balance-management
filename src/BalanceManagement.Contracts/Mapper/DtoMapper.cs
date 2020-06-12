using AutoMapper;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Data.Entities;
using BalanceManagement.Data.Types;

namespace BalanceManagement.Contracts.Mapper
{
    /// <summary>
    ///     Configuration of Dtos
    /// </summary>
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>()
                .ForMember(f=>f.Password,map=> map.Ignore());
        }
    }
}