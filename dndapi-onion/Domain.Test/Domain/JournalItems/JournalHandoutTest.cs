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
            var permission = new JournalItemPermissionDto
            {
                UserId = userId,
                CanSee = true,
                CanEdit = true
            };
            handoutModel = new JournalHandoutDto
            {
                Name = "handout",
                Description = "description",
                OwnerNotes = "ownerNotes",
                Permissions = new List<JournalItemPermissionDto> { permission }
            };

            sut = new JournalHandout(handoutModel, Guid.NewGuid());
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
            var permission1 = new JournalItemPermissionDto
            {
                UserId = userId,
                CanSee = true,
                CanEdit = true
            };

            var permission2 = new JournalItemPermissionDto
            {
                UserId = newUserId,
                CanSee = true,
                CanEdit = true
            };

            JournalHandoutDto updateModel = new JournalHandoutDto
            {
                Name = "updated",
                Description = "updated",
                OwnerNotes = "updated",
                Permissions = new List<JournalItemPermissionDto> { permission1, permission2 }
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
