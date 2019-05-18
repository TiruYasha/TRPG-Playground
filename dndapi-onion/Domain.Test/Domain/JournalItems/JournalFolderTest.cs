using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading.Tasks;

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
        public async Task AddJournalItemAsync()
        {
            var journalItem = new AddJournalItemDto()
            {
                JournalItem = new JournalFolderDto() { Name = "test" },
                ParentFolderId = Guid.Empty
            };

            var sut = new JournalFolder();

            var result = await sut.AddJournalItem(journalItem);

            sut.JournalItems.Count.ShouldBe(1);
            result.Name.ShouldBe(journalItem.JournalItem.Name);
        }


        [TestMethod]
        public void AddJournalItemOnNullThrowArgumentNullException()
        {
            var sut = new JournalFolder();

            var result = Should.Throw<ArgumentNullException>(async () => await sut.AddJournalItem(null));
             
            result.ParamName.ShouldBe("dto");
        }
    }
}
