using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Test.Domain.JournalItems
{
    [TestClass]
    public class JournalHandoutTest
    {
        [TestMethod]
        public void ConstructorSetsTheHandoutData()
        {
            var model = new AddJournalItemModel
            {
                Name = "test"
            };

            var result = new JournalHandout(model);

            result.Type.ShouldBe(JournalItemType.Handout);
            result.Name.ShouldBe(model.Name);
        }
    }
}
