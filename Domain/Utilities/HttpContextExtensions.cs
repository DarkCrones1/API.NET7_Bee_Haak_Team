
using Microsoft.EntityFrameworkCore;
using Web_API_Bee_Haak.DTOS;

namespace Web_API_Bee_Haak.Utilities;
public static class HttpContextExtensions
{
    public async static Task InsertPaginationsData<T>(this HttpContext httpContext, IQueryable<T> queryable)
    {
        if (httpContext == null) {throw new ArgumentNullException(nameof(httpContext));}

        double Cuantity = await queryable.CountAsync();
        httpContext.Response.Headers.Add("TotalCuantityRegisters", Cuantity.ToString());
    }
}