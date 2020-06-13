using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Contracts.Dtos.Users;

namespace BalanceManagement.Service.IService
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(string userName, string password);
        Task<UserDto> AddAsync(UserDto user);
        Task<UserDto> UpdateAsync(UserDto user);
        Task<bool> DeleteAsync(int id);
        Task<UserDto> GetByIdAsync(int id);
        Task<PagedCollection<UserDto>> GetListAsync (PagedFilter pagedFilter);
        Task<bool> ExistsUser(UserDto user);
    }
}
