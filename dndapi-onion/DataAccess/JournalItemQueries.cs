using System;
using System.Linq;
using Domain.Domain.JournalItems;

namespace DataAccess
{
    public static class JournalItemQueries
    {
        public static IQueryable<JournalFolder> FilterById(this IQueryable<JournalFolder> journalFolder, Guid id)
        {
            return journalFolder.Where(j => j.Id == id);
        }

        public static IQueryable<JournalItem> FilterByParentFolderId(this IQueryable<JournalItem> queryable, Guid? id)
        {
            return queryable.Where(j => j.ParentFolderId == id);
        }

        public static IQueryable<JournalItem> FilterByGameId(this IQueryable<JournalItem> queryable, Guid? id)
        {
            return queryable.Where(j => j.GameId == id);
        }
    }
}