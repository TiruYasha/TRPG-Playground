﻿using Domain.Domain.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;
using Domain.Exceptions;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalFolderTest
    {
        private JournalFolder sut;

        private AddJournalItemDto baseDto;

        [TestInitialize]
        public void Initialize()
        {
            baseDto = new AddJournalItemDto
            {
                JournalItem = new JournalFolderDto
                {
                    Name = "test"
                }
            };
            sut = new JournalFolder(baseDto.JournalItem, Guid.NewGuid());
        }

        [TestMethod]
        public void ConstructorSetTheTypeToFolder()
        {
            sut.Type.ShouldBe(JournalItemType.Folder);
        }

        [TestMethod]
        public void ConstructorInitializesTheJournalItemsList()
        {
            sut.JournalItems.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task AddJournalItemAsync()
        {
            var journalItem = new AddJournalItemDto()
            {
                JournalItem = new JournalFolderDto() { Name = "test" },
                ParentFolderId = Guid.Empty
            };

            var result = await sut.AddJournalItem(journalItem.JournalItem, Guid.Empty);

            sut.JournalItems.Count.ShouldBe(1);
            result.Name.ShouldBe(journalItem.JournalItem.Name);
        }

        [TestMethod]
        public void AddJournalItemOnNullThrowArgumentNullException()
        {
            var result = Should.Throw<ArgumentNullException>(async () => await sut.AddJournalItem(null, Guid.Empty));
             
            result.ParamName.ShouldBe("dto");
        }

        [TestMethod]
        public async Task UpdateUpdatesAllTheValues()
        {
            // arrange
            var oldUpdatedTime = sut.LastEditedOn;
            var journalItemDto = new JournalFolderDto
            {
                Name =  "Updated"
            };

            // act
            await sut.Update(journalItemDto);

            // assert
            sut.Name.ShouldBe(journalItemDto.Name);
            sut.LastEditedOn.ShouldBeGreaterThan(oldUpdatedTime);
        }

        [TestMethod]
        public void UpdateShouldThrowJournalItemExceptionOnEmptyName()
        {
            // arrange
            var journalItemDto = new JournalFolderDto
            {
                Name = ""
            };

            // act
            var result = Should.Throw<JournalItemException>(async () => { await sut.Update(journalItemDto); });

            // assert
            result.Message.ShouldBe("The name is empty");
        }
    }
}
