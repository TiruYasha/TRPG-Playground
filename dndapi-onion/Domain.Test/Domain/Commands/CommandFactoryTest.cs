using Domain.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Test.Domain.Commands
{
    [TestClass]
    public class CommandFactoryTest
    {
        [TestMethod]
        public void NoCommandCreatesDefaultCommand()
        {
            var result = CommandFactory.Create("hallo");

            result.ShouldBeOfType<DefaultCommand>();
        }

        [TestMethod]
        public void RollCommandCreatesNormallDiceRollCommand()
        {
            var result = CommandFactory.Create("/r 1d20");

            result.ShouldBeOfType<NormalDiceRollCommand>();
        }
    }
}
