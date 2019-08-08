﻿using Domain.Domain;
using Domain.Domain.Commands;
using Domain.Domain.JournalItems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Domain.Domain.Layers;
using Microsoft.AspNetCore.Identity;
using Domain.Domain.PlayArea;

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
        public DbSet<JournalCharacterSheet> JournalCharacterSheets { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Layer> Layers { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<CharacterToken> CharacterTokens { get; set; }

        public DndContext(DbContextOptions<DndContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetupGamePlayer(modelBuilder);
            SetupJournalItemPermission(modelBuilder);

            modelBuilder.Entity<JournalFolder>().HasMany(j => j.JournalItems).WithOne(j => j.ParentFolder)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Map>().HasMany(m => m.Layers).WithOne(l => l.Map)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Game>().HasOne(g => g.VisibleMap);
            modelBuilder.Entity<Game>().HasMany(g => g.Maps).WithOne(m => m.Game);
        }

        private void SetupJournalItemPermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalItemPermission>()
                .HasKey(j => new { j.JournalItemId, j.UserId, j.GameId });

            modelBuilder.Entity<JournalItemPermission>()
                .HasOne(j => j.JournalItem)
                .WithMany(j => j.Permissions)
                .HasForeignKey(j => j.JournalItemId);
            modelBuilder.Entity<JournalItemPermission>()
                .HasOne(j => j.GamePlayer)
                .WithMany(j => j.JournalItemPermissions)
                .HasForeignKey(j => new { j.GameId, j.UserId });
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

        public async Task EnsureSeeded(UserManager<User> userManager)
        {
            var user = new User
            {
                Email = "test@test.nl",
                UserName = "test@test.nl"
            };

            if (await this.Users.AnyAsync(u => u.Email == user.Email))
            {
                return;
            }

            var result = await userManager.CreateAsync(user, "test12");

            var game = new Game("testGame", user.Id);

            await this.AddAsync(game);

            await this.SaveChangesAsync();
        }
    }
}
