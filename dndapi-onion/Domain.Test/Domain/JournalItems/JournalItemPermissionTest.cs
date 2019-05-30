using Domain.Domain.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalItemPermissionTest
    {

        private Guid journalItemId = Guid.NewGuid();
        private Guid userId = Guid.NewGuid();
        private Guid gameId = Guid.NewGuid();

        [TestMethod]
        public void ConstructorSetTheValues()
        {
            var canSee = true;
            var canEdit = true;

            var result = new JournalItemPermission(journalItemId, userId, gameId, canSee, canEdit);

            result.JournalItemId.ShouldBe(journalItemId);
            result.UserId.ShouldBe(userId);
            result.GameId.ShouldBe(gameId);
            result.CanSee.ShouldBe(canSee);
            result.CanEdit.ShouldBe(canEdit);
        }

        [TestMethod]
        public void ConstructorCanEditDefaultValueIsFalse()
        {
            var canSee = true;

            var result = new JournalItemPermission(journalItemId, userId, gameId, canSee);

            result.CanEdit.ShouldBe(false);
        }

        [TestMethod]
        public void ConstructorSetCanSeeTrueWhenCanEditIsTrue()
        {
            var canSee = false;

            var result = new JournalItemPermission(journalItemId, userId, gameId, canSee, true);

            result.CanSee.ShouldBe(true);
        }
    }
}
