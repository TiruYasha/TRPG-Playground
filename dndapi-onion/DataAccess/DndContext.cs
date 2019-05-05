using Domain.Domain;
using Domain.Domain.Commands;
using Domain.Domain.JournalItems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class DndContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<DefaultCommand> DefaultCommands { get; set; }
        public DbSet<NormalDiceRollCommand> NormalDiceRollCommands { get; set; }
        public DbSet<Command> Commands { get; set; } 
        public DbSet<JournalItem> JournalItems { get; set; }
        public DbSet<JournalFolder> JournalFolders { get; set; }
        public DbSet<JournalHandout> JournalHandouts { get; set; }

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

            modelBuilder.Entity<JournalItemPemission>()
               .HasKey(j => new { j.JournalItemId, j.GameId, j.UserId });

            modelBuilder.Entity<JournalItemPemission>()
                .HasOne(j => j.JournalItem)
                .WithMany(j => j.Pemissions)
                .HasForeignKey(j => j.JournalItemId);
            modelBuilder.Entity<JournalItemPemission>()
                .HasOne(j => j.Player)
                .WithMany(j => j.JournalItemPermissions)
                .HasForeignKey(j => new { j.GameId, j.UserId });
        }
    }
}
