using MoviesApi.Dtos;

namespace MoviesApi.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto pagination)
        {
            return queryable
                .Skip((pagination.CurrentPage - 1) * pagination.RegistersPerPage)
                .Take(pagination.RegistersPerPage);
        }
    }
}
