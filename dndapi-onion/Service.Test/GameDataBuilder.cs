using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.Domain.Layers;
using Domain.Dto.RequestDto;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;

namespace Service.Test
{
    public class GameDataBuilder
    {
        private readonly Queue<Func<Task>> actions;
        private Game game;

        public User Player1 { get; private set; }
        public User Player2 { get; private set; }

        public GameDataBuilder()
        {
            actions = new Queue<Func<Task>>();
        }

        public GameDataBuilder WithJournalFolder(bool withNestedItems = false)
        {
            actions.Enqueue(async () =>
            {
                var dto = new JournalFolderDto
                {
                    Name = "TestingFolder"
                };

                var item = await game.AddJournalItem(dto) as JournalFolder;

                if (withNestedItems)
                {
                    var nestedFolder = new JournalFolderDto
                    {
                        Name = "TestingFolder"
                    };

                    var nestedHandout = new JournalHandoutDto()
                    {
                        Name = "TestingHandout",
                        Description = "This is a description",
                        OwnerNotes = "These are ownernotes",
                        ImageId = Guid.NewGuid()
                    };

                    await item.AddJournalItem(nestedFolder, game.Id);
                    await item.AddJournalItem(nestedHandout, game.Id);
                }
            });

            return this;
        }

        public GameDataBuilder WithJournalHandout(bool withPlayers = false)
        {
            actions.Enqueue(async () =>
            {
                var journalHandout = new JournalHandoutDto()
                {
                    Name = "TestingHandout",
                    Description = "This is a description",
                    OwnerNotes = "These are ownernotes",
                    ImageId = Guid.NewGuid()
                };

                if (withPlayers)
                {
                    await AddPlayers();

                    journalHandout.Permissions.Add(
                        new JournalItemPermissionDto
                        {
                            UserId = Player1.Id,
                            CanSee = true
                        });

                    journalHandout.Permissions.Add(
                        new JournalItemPermissionDto
                        {
                            UserId = Player2.Id,
                            CanSee = true,
                            CanEdit = true
                        });
                }

                await game.AddJournalItem(journalHandout);
            });

            return this;
        }

        public GameDataBuilder WithMaps(bool withExtraLayers = false)
        {
            actions.Enqueue(async () =>
            {
                var mapDto = new MapDto
                {
                    GridSizeInPixels = 40,
                    HeightInPixels = 400,
                    Name = "test",
                    WidthInPixels = 400
                };

                var mapDto2 = new MapDto
                {
                    GridSizeInPixels = 50,
                    HeightInPixels = 500,
                    Name = "test2",
                    WidthInPixels = 500
                };

                var map1 = await game.AddMap(mapDto);
                var map2 = await game.AddMap(mapDto2);

                if (withExtraLayers)
                {
                    var layer2 = new LayerDto
                    {
                        Name = "layer2",
                        Order = 1,
                        Type = LayerType.Default
                    };

                    var layer3 = new LayerDto
                    {
                        Name = "layer3",
                        Order = 1,
                        Type = LayerType.Default
                    };

                    await map1.AddLayer(layer2);
                    await map2.AddLayer(layer3);
                }
            });

            return this;
        }

        public Task<Game> BuildGame()
        {
            return Task.Run(async () =>
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "owner",
                };

                game = new Game("testgame", user);

                foreach (var action in actions)
                {
                    await action();
                }

                return game;
            });
        }

        private async Task AddPlayers()
        {
            if (!game.Players.Any())
            {
                Player1 = new User { Id = Guid.NewGuid(), UserName = "canSeeJournalHandout" };
                Player2 = new User { Id = Guid.NewGuid(), UserName = "canEditJournalHandout" };

                await game.Join(Player1);
                await game.Join(Player2);
            }
        }
    }
}
