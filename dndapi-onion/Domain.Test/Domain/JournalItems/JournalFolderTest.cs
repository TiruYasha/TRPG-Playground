using Domain.Domain.JournalItems;
using Domain.Exceptions;
using Domain.RequestModels.Journal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalFolderTest
    {
        [TestMethod]
        public void ConstructorSetTheTypeToFolder()
        {
            var result = new JournalFolder();

            result.Type.ShouldBe(JournalItemType.Folder);
        }

        [TestMethod]
        public void ConstructorInitializesTheJournalItemsList()
        {
            var result = new JournalFolder();

            result.JournalItems.ShouldNotBeNull();
        }

        [TestMethod]
        public void AddJournalItemAddsTheJournalItem()
        {
            var journalItem = new JournalFolder();

            var sut = new JournalFolder();

            sut.AddJournalItem(journalItem);

            sut.JournalItems.Count.ShouldBe(1);
        }

        [TestMethod]
        public void AddJournalItemOnNullThrowArgumentNullException()
        {
            var sut = new JournalFolder();

            var result = Should.Throw<ArgumentNullException>(() => sut.AddJournalItem(null));

            result.ParamName.ShouldBe("item");
        }
    }
}
