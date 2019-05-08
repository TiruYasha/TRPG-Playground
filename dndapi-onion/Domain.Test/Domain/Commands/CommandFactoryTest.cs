using Domain.Domain.Commands;
using Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

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

        [TestMethod]
        public void FactoryOnlySplitsOnFirstWhitespace()
        {
            var command = CommandFactory.Create("/r 1 d 1") as NormalDiceRollCommand;
            command.Execute();

            command.RollResult.ShouldBe(1);
        }

        [TestMethod]
        public void FactoryShouldThrowExceptionIfThereAreNoArguments()
        {
            var result = Should.Throw<NoArgumentsException>(() => CommandFactory.Create("/asdsa"));

            result.Message.ShouldBe("Please provide arguments");
        }

        [TestMethod]
        public void FactoryShouldThrowExceptionIfCommandDoesNotExist()
        {
            var message = "/dsfdsf dsaf sdf";

            var result = Should.Throw<UnrecognizedCommandException>(() => CommandFactory.Create(message));

            result.Message.ShouldBe("Unrecognized command: " + message);
        }
    }
}
