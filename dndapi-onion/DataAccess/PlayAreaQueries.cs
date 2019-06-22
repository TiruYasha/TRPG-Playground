using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Domain;

namespace DataAccess
{
    public static class PlayAreaQueries
    {
        public static IQueryable<PlayArea> FilterByPlayAreaId(this IQueryable<PlayArea> queryable, Guid id)
        {
            return queryable.Where(g => g.Id == id);
        }

        public static IQueryable<PlayArea> FilterByGameId(this IQueryable<PlayArea> queryable, Guid id)
        {
            return queryable.Where(g => g.GameId == id);
        }

        public static IQueryable<PlayArea> FilterByGameAndPlayAreaId(this IQueryable<PlayArea> queryable, Guid gameId, Guid playAreaId)
        {
            return queryable.FilterByGameId(gameId).FilterByPlayAreaId(playAreaId);
        }
    }
}
