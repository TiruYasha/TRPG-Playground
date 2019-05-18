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
    }
}