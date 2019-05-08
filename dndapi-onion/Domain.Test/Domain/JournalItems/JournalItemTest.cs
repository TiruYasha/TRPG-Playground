using Domain.Domain.JournalItems;
using Domain.Exceptions;
using Domain.Test.Domain.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var itemType = JournalItemType.Handout;
            var name = "test";
            var gameId = Guid.NewGuid();
            var imageId = "path";
            var canSee = new List<Guid>() { user1Guid, user2Guid };
            var canEdit = new List<Guid>() { user1Guid };

            var result = new JournalItemMock(itemType, name, gameId, imageId, canSee, canEdit);

            result.Type.ShouldBe(itemType);
            result.Name.ShouldBe(name);
            result.GameId.ShouldBe(gameId);
            result.ImageId.ShouldBe(imageId);

            result.Permissions.Count.ShouldBe(2);
            result.Permissions.FirstOrDefault().CanSee.ShouldBe(true);
            result.Permissions.FirstOrDefault().CanEdit.ShouldBe(true);
            result.Permissions.LastOrDefault().CanSee.ShouldBe(true);
            result.Permissions.LastOrDefault().CanEdit.ShouldBe(false);
        }

        [TestMethod]
        public void ConstructorThrowsJournalItemExceptionOnEmptyName()
        {
            var result = Should.Throw<JournalItemException>(() => new JournalItemMock(JournalItemType.Folder, "", Guid.NewGuid(), null, null, null));

            result.Message.ShouldBe("The name is empty");
        }
    }
}
