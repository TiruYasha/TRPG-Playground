using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DndContext : IdentityDbContext<DndUser>
    {
        public DndContext(DbContextOptions<DndContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CommandResult> CommandResults { get; set; }
        public DbSet<NormalRollCommandResult> NormalRollCommandResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<ChatMessage>()
               .Property(b => b.CreatedDate)
               .HasDefaultValueSql("now()");
        }
    }
}
