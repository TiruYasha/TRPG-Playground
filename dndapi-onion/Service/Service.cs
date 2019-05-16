using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public abstract class Service : IDisposable
    {
        internal readonly DndContext context;

        internal Service(DbContextOptions<DndContext> options)
        {
            context = new DndContext(options);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
