using Logic.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using Shouldly;
using System.Collections.Generic;

namespace Logic.Test.Helpers
{
    [TestClass]
    public class CommandHelperTest
    {
        private ICommandHelper _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new CommandHelper();
        }

        [TestMethod]
        public void CheckIfMessageIsCommandGivenRollCommandThenReturnTrue()
        {
            var message = "/roll 1d20";

            bool result = _sut.CheckIfMessageIsCommand(message);

            result.ShouldBe(true);
        }

        [TestMethod]
        public void CheckIfMessageIsCommandGivenWrongRollCommandThenReturnFalse()
        {
            var message = "roll 1d20";

            bool result = _sut.CheckIfMessageIsCommand(message);

            result.ShouldBe(false);
        }

        [TestMethod]
        public void RunCommandGivenRollCommandThenReturnRollResultFor1d1()
        {
            var message = "/roll 1d1";

            for (int i = 0; i < 50; i++)
            {
                CommandResult result = _sut.RunCommand(message);

                result.ShouldBeOfType<NormalRollCommandResult>();
                ((NormalRollCommandResult)result).Result.ShouldBeGreaterThanOrEqualTo(1);
                ((NormalRollCommandResult)result).Result.ShouldBeLessThanOrEqualTo(1);
            }
        }

        [TestMethod]
        public void RunCommandGivenRollCommandThenReturnRollResultFor1d20()
        {
            var message = "/roll 1d20";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                CommandResult result = _sut.RunCommand(message);

                result.ShouldBeOfType<NormalRollCommandResult>();

                allResults.Add(((NormalRollCommandResult)result).Result);

                ((NormalRollCommandResult)result).Result.ShouldBeGreaterThanOrEqualTo(1);
                ((NormalRollCommandResult)result).Result.ShouldBeLessThanOrEqualTo(20);
            }

            for(int i = 1; i <= 20; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void RunCommandGivenRollCommandWithOperatorThenReturnRollResultFor1d20Plus5()
        {
            var message = "/roll 1d20+5";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                CommandResult result = _sut.RunCommand(message);

                result.ShouldBeOfType<NormalRollCommandResult>();

                allResults.Add(((NormalRollCommandResult)result).Result);

                ((NormalRollCommandResult)result).Result.ShouldBeGreaterThanOrEqualTo(1+5);
                ((NormalRollCommandResult)result).Result.ShouldBeLessThanOrEqualTo(20+5);
            }

            for (int i = 1+5; i <= 20+5; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void RunCommandGivenRollCommandWithOperatorThenReturnRollResultFor1d20Minus5()
        {
            var message = "/roll 1d20-5";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                CommandResult result = _sut.RunCommand(message);

                result.ShouldBeOfType<NormalRollCommandResult>();

                allResults.Add(((NormalRollCommandResult)result).Result);

                ((NormalRollCommandResult)result).Result.ShouldBeGreaterThanOrEqualTo(1 - 5);
                ((NormalRollCommandResult)result).Result.ShouldBeLessThanOrEqualTo(20 - 5);
            }

            for (int i = 1 - 5; i <= 20 - 5; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void RunCommandGivenRollCommandWithMultipleDiceThenReturnRollResultFor2d20Minus5()
        {
            var message = "/roll 2d20-5";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                CommandResult result = _sut.RunCommand(message);

                result.ShouldBeOfType<NormalRollCommandResult>();

                allResults.Add(((NormalRollCommandResult)result).Result);

                ((NormalRollCommandResult)result).Result.ShouldBeGreaterThanOrEqualTo(2 - 5);
                ((NormalRollCommandResult)result).Result.ShouldBeLessThanOrEqualTo(40 - 5);
            }

            for (int i = 2 - 5; i <= 40 - 5; i++)
            {
                allResults.ShouldContain(i);
            }
        }
    }
}
