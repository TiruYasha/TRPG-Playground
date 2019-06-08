using Domain.Domain.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;
using Domain.Exceptions;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalHandoutTest
    {
        private readonly Guid userId = Guid.NewGuid();
        private JournalHandoutDto handoutModel;
        private AddJournalItemDto model;

        private JournalHandout sut;

        [TestInitialize]
        public void Initialize()
        {
            handoutModel = new JournalHandoutDto
            {
                Name = "handout",
                Description = "description",
                OwnerNotes = "ownerNotes",
                CanSee = new List<Guid> { userId },
                CanEdit = new List<Guid> { userId }
            };

            model = new AddJournalItemDto
            {
                JournalItem = handoutModel
            };

            sut = new JournalHandout(model, Guid.NewGuid());
        }

        [TestMethod]
        public void ConstructorSetsTheHandoutData()
        {
            sut.Type.ShouldBe(JournalItemType.Handout);
            sut.Name.ShouldBe(handoutModel.Name);
            sut.Description.ShouldBe(handoutModel.Description);
            sut.OwnerNotes.ShouldBe(handoutModel.OwnerNotes);
            sut.Permissions.Count.ShouldBe(1);
        }

        [TestMethod]
        public async Task UpdateUpdatesTheHandout()
        {
            // arrange
            var newUserId = Guid.NewGuid();
            JournalHandoutDto updateModel = new JournalHandoutDto
            {
                Name = "updated",
                Description = "updated",
                OwnerNotes = "updated",
                CanSee = new List<Guid> { userId, newUserId },
                CanEdit = new List<Guid> { userId, newUserId }
            };

            // act
            await sut.Update(updateModel);

            // assert
            sut.Name.ShouldBe(updateModel.Name);
            sut.Description.ShouldBe(updateModel.Description);
            sut.OwnerNotes.ShouldBe(updateModel.OwnerNotes);
            sut.Permissions.Count.ShouldBe(2);
        }

        [TestMethod]
        public void UpdateShouldThrowJournalItemExceptionOnEmptyName()
        {
            // arrange
            var journalItemDto = new JournalHandoutDto()
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
