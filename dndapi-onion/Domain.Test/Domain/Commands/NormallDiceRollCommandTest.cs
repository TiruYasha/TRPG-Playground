using Domain.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Test.Domain.Commands
{
    [TestClass]
    public class NormallDiceRollCommandTest
    {
        [TestMethod]
        public void ConstructorSetTheTypeOnNormalDiceRoll()
        {
            var result = new NormalDiceRollCommand();

            result.Type.ShouldBe(CommandType.NormallDiceRoll);
        }
    }
}
