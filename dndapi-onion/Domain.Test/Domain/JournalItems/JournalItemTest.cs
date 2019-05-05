using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.Exceptions;
using Domain.Test.Domain.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalItemTest
    {
        [TestMethod]
        public void ConstructorSetsAllProperties()
        {
            var itemType = JournalItemType.Handout;
            var name = "test";
            var imagePath = "path";
            var canSee = new List<User>();
            var canEdit = new List<User>();

            var result = new JournalItemMock(itemType, name, imagePath, canSee, canEdit);

            result.Type.ShouldBe(itemType);
            result.Name.ShouldBe(name);
            result.ImagePath.ShouldBe(imagePath);
            result.CanSee.ShouldBe(canSee);
            result.CanEdit.ShouldBe(canEdit);
        }

        [TestMethod]
        public void ConstructorThrowsJournalItemExceptionOnEmptyName()
        {
            var result = Should.Throw<JournalItemException>(() => new JournalItemMock(JournalItemType.Folder, "", null, null, null));

            result.Message.ShouldBe("The name is empty");
        }
    }
}
