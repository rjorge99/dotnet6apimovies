using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Helpers
{
    public static class HttpContextExtensions
    {
        public static async Task InsertPaginationParams<T>(this HttpContext httpContext, IQueryable<T> queryable,
            int registersPerPage)
        {
            double quantity = await queryable.CountAsync();
            var totalPages = Math.Ceiling(quantity / registersPerPage);

            httpContext.Response.Headers.Add("totalPages", totalPages.ToString());

        }
    }
}
