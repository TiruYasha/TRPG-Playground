using Domain.Domain.JournalItems;
using Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Dto.Shared;
using Domain.Mocks;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalItemTest
    {
        [TestMethod]
        public void ConstructorSetsAllProperties()
        {
            var user1Guid = Guid.NewGuid();
            var user2Guid = Guid.NewGuid();

            var gameId = Guid.NewGuid();
            
            var dto = new JournalItemDto
            {
                Type = JournalItemType.Handout,
                Name = "test",
                Permissions = new List<JournalItemPermissionDto>()
                {
                    new JournalItemPermissionDto
                    {
                        CanEdit = true,
                        CanSee = true,
                        UserId = user1Guid
                    },
                    new JournalItemPermissionDto
                    {
                        CanEdit = false,
                        CanSee = true,
                        UserId = user2Guid
                    }
                }
            };

            var result = new JournalItemMock(dto, gameId);

            result.Type.ShouldBe(dto.Type);
            result.Name.ShouldBe(dto.Name);
            result.GameId.ShouldBe(gameId);

            result.Permissions.Count.ShouldBe(2);
            result.Permissions.FirstOrDefault().CanSee.ShouldBe(true);
            result.Permissions.FirstOrDefault().CanEdit.ShouldBe(true);
            result.Permissions.LastOrDefault().CanSee.ShouldBe(true);
            result.Permissions.LastOrDefault().CanEdit.ShouldBe(false);
        }

        [TestMethod]
        public void ConstructorThrowsJournalItemExceptionOnEmptyName()
        {
            var dto = new JournalItemDto
            {
                Type = JournalItemType.Handout,
                Name = ""
            };

            var result = Should.Throw<JournalItemException>(() => new JournalItemMock(dto, Guid.NewGuid()));

            result.Message.ShouldBe("The name is empty");
        }
    }
}
