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

            SetupGamePlayer(modelBuilder);
        }

        private static void SetupGamePlayer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamePlayer>()
                            .HasKey(gp => new { gp.GameId, gp.UserId });
            modelBuilder.Entity<GamePlayer>()
                .HasOne(gp => gp.Game)
                .WithMany(g => g.Players)
                .HasForeignKey(gp => gp.GameId);
            modelBuilder.Entity<GamePlayer>()
                .HasOne(gp => gp.User)
                .WithMany(g => g.JoinedGames)
                .HasForeignKey(gp => gp.UserId);
        }
    }
}
