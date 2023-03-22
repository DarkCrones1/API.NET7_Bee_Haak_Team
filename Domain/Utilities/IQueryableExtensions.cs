using System.Linq;
using Web_API_Kaab_Haak.DTOS;
namespace Web_API_Kaab_Haak.Domain.Utilities;
public static class IQueryableExtensions
{
    public static IQueryable<T> Page<T> (this IQueryable<T> queryable, PaginationDTO paginationDTO)
    {
        return queryable.Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPage)
        .Take(paginationDTO.RecordsPage);
    }
}