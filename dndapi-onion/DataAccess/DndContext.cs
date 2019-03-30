using Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class DndContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Game> Games { get; set; }

        public DndContext(DbContextOptions<DndContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ChatMessage>()
            //   .Property(b => b.CreatedDate)
            //   .HasDefaultValueSql("now()");
        }
    }
}
