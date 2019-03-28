using Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DndContext : IdentityDbContext<User>
    {
        public DndContext(DbContextOptions<DndContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ChatMessage>()
            //   .Property(b => b.CreatedDate)
            //   .HasDefaultValueSql("now()");
        }
    }
}
