using System.Collections.Generic;

namespace BalanceManagement.Contracts.Dtos
{
    public class PagedCollection<TDto>
    {
        public List<TDto> Items { get; set; }
        public long Total { get; set; }
    }
}
