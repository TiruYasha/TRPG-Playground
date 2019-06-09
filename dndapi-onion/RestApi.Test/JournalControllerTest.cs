using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Domain.JournalItems;
using Domain.Dto.ReturnDto.Journal;
using Domain.Dto.Shared;
using Domain.Events;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Hubs;
using RestApi.Utilities;
using Shouldly;

namespace RestApi.Test
{
    [TestClass]
    public class JournalControllerTest
    {
        private JournalController sut;

        private Mock<IJournalService> journalService;
        private Mock<IJwtReader> jwtReader;
        private Mock<IHubContext<GameHub>> hubContext;

        [TestInitialize]
        public void Initialize()
        {
            journalService = new Mock<IJournalService>(MockBehavior.Strict);
            jwtReader = new Mock<IJwtReader>(MockBehavior.Strict);
            hubContext = new Mock<IHubContext<GameHub>>(MockBehavior.Strict);

            sut = new JournalController(journalService.Object, jwtReader.Object, hubContext.Object);
        }

        [TestMethod]
        public async Task UpdateJournalItemReturnsOkayAndSendsEvents()
        {
            // arrange
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var dto = new JournalItemDto
            {
                Id = Guid.NewGuid(),
                Type = JournalItemType.Handout
            };

            var treeItemDto = new JournalItemTreeItemDto
            {
                Type = JournalItemType.Handout
            };

            var permission = new JournalItemPermission
            {
                UserId = Guid.NewGuid(),
                CanEdit = true
            };

            jwtReader.Setup(s => s.GetUserId()).Returns(userId);
            jwtReader.Setup(s => s.GetGameId()).Returns(gameId);

            journalService.Setup(j => j.UpdateJournalItem(dto, gameId, userId)).ReturnsAsync(treeItemDto);
            journalService.Setup(j => j.GetJournalItemPermissions(dto.Id)).ReturnsAsync(new[] { permission });

            hubContext.Setup(h =>
                    h.Clients.User(permission.UserId.ToString())
                        .SendCoreAsync("JournalItemUpdated", new object[] {treeItemDto}, default))
                .Returns(Task.CompletedTask);
            hubContext.Setup(h =>
                    h.Clients.User(userId.ToString())
                        .SendCoreAsync("JournalItemUpdated", new object[] {treeItemDto}, default))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.UpdateJournalItem(dto);

            // assert
            result.ShouldBeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateJournalItemReturnsOkayAndSendsEventsToGroupIfFolder()
        {
            // arrange
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var dto = new JournalItemDto
            {
                Id = Guid.NewGuid(),
            };

            var treeItemDto = new JournalItemTreeItemDto();

            var permission = new JournalItemPermission
            {
                UserId = Guid.NewGuid(),
            };

            jwtReader.Setup(s => s.GetUserId()).Returns(userId);
            jwtReader.Setup(s => s.GetGameId()).Returns(gameId);

            journalService.Setup(j => j.UpdateJournalItem(dto, gameId, userId)).ReturnsAsync(treeItemDto);
            journalService.Setup(j => j.GetJournalItemPermissions(dto.Id)).ReturnsAsync(new[] { permission });

            hubContext.Setup(h =>
                    h.Clients.Group(gameId.ToString())
                        .SendCoreAsync(JournalEvents.JournalItemUpdated, new object[] { treeItemDto }, default))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.UpdateJournalItem(dto);

            // assert
            result.ShouldBeOfType<OkResult>();
        }
    }
}
