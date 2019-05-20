using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

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
