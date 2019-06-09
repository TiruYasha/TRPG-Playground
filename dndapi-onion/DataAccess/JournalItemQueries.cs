using System;
using System.Linq;
using Domain.Domain.JournalItems;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public static class JournalItemQueries
    {
        public static IQueryable<JournalItem> FilterById(this IQueryable<JournalItem> journalFolder, Guid id)
        {
            return journalFolder.Where(j => j.Id == id);
        }

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

        public static IQueryable<JournalItem> FilterByCanSee(this IQueryable<JournalItem> queryable, Guid? userId)
        {
            return queryable.Include(j => j.Permissions).Include(j => j.Game)
                .Where(j => j.Game.OwnerId == userId || j.Permissions.Any(p => p.UserId == userId && p.CanSee));
        }

        public static IQueryable<JournalItem> FilterByCanEdit(this IQueryable<JournalItem> queryable, Guid? userId)
        {
            return queryable.Include(j => j.Permissions).Include(j => j.Game)
                .Where(j => j.Game.OwnerId == userId || j.Permissions.Any(p => p.UserId == userId && p.CanEdit));
        }
    }
}