using Microsoft.EntityFrameworkCore;

namespace TextToSQL.Extensions;

public static class QuerableExttensions
{
    public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> query, string includes) where T : class
    {
        if (string.IsNullOrWhiteSpace(includes))
        {
            return query;
        }

        var includeList = includes.Split(',');
        return includeList.Aggregate(query, (current, include) => current.Include(include.Trim()));
    }
}