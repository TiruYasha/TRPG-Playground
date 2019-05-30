using Domain.Domain.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalHandoutTest
    {
        [TestMethod]
        public void ConstructorSetsTheHandoutData()
        {
            var handoutModel = new JournalHandoutDto
            {
                Name = "handout",
                Description = "description",
                OwnerNotes = "ownerNotes"
            };

            var model = new AddJournalItemDto
            {
               JournalItem = handoutModel
            };

            var result = new JournalHandout(model, Guid.NewGuid());

            result.Type.ShouldBe(JournalItemType.Handout);
            result.Name.ShouldBe(handoutModel.Name);
            result.Description.ShouldBe(handoutModel.Description);
            result.OwnerNotes.ShouldBe(handoutModel.OwnerNotes);
        }
    }
}
