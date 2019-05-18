using Domain.Domain;
using System;
using System.Linq;

namespace DataAccess
{
    public static class GameQueries
    {
        public static IQueryable<Game> FilterById(
            this IQueryable<Game> queryable,
            Guid gameId)
        {
            return queryable.Where(g => g.Id == gameId);
        }

        public static IQueryable<Game> FilterOnOwnerById(this IQueryable<Game> queryable, Guid id)
        {
            return queryable.Where(g => g.Owner.Id == id);
        }
    }
}
