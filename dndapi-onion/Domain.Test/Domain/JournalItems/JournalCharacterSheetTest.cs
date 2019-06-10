using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Domain.JournalItems;
using Domain.Dto.Shared;
using Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalCharacterSheetTest
    {
        private readonly Guid userId = Guid.NewGuid();
        private JournalCharacterSheetDto characterSheetModel;

        private JournalCharacterSheet sut;

        [TestInitialize]
        public void Initialize()
        {
            var permission = new JournalItemPermissionDto
            {
                UserId = userId,
                CanSee = true,
                CanEdit = true
            };

            characterSheetModel = new JournalCharacterSheetDto
            {
                Name = "handout",
                Description = "description",
                OwnerNotes = "ownerNotes",
                Permissions = new List<JournalItemPermissionDto> { permission }
            };

            sut = new JournalCharacterSheet(characterSheetModel, Guid.NewGuid());
        }

        [TestMethod]
        public void ConstructorSetsTheCharacterSheetData()
        {
            sut.Type.ShouldBe(JournalItemType.CharacterSheet);
            sut.Name.ShouldBe(characterSheetModel.Name);
            sut.Description.ShouldBe(characterSheetModel.Description);
            sut.OwnerNotes.ShouldBe(characterSheetModel.OwnerNotes);
            sut.Permissions.Count.ShouldBe(1);
        }

        [TestMethod]
        public async Task UpdateUpdatesTheCharacterSheet()
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

            JournalCharacterSheetDto updateModel = new JournalCharacterSheetDto
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

        [TestMethod]
        public async Task SetTokenSetsTheToken()
        {
            // arrange
            var extension = ".png";
            var originalName = "testing.png";

            // act
            var result = await sut.SetToken(extension, originalName);

            // assert
            sut.Token.ShouldBe(result);
        }
    }
}
