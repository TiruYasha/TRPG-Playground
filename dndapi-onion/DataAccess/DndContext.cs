using Domain.Domain;
using Domain.Domain.Commands;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class DndContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<DefaultCommand> DefaultCommands { get; set; }
        public DbSet<NormalDiceRollCommand> NormalDiceRollCommands { get; set; }
        public DbSet<Command> Commands { get; set; } 

        public DndContext(DbContextOptions<DndContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
