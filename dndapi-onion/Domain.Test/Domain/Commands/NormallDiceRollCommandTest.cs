using Domain.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Domain.Test.Domain.Commands
{
    [TestClass]
    public class NormallDiceRollCommandTest
    {
        [TestMethod]
        public void ConstructorSetTheTypeOnNormalDiceRoll()
        {
            var arguments = "";
            
            var result = new NormalDiceRollCommand(arguments);

            result.Type.ShouldBe(CommandType.NormallDiceRoll);
            result.CommandText.ShouldBe(arguments);
        }

        [TestMethod]
        public void ExecuteRollsA1d1AlwaysRolls1()
        {
            var commandText = "1d1";

            for (int i = 0; i < 50; i++)
            {
                var command = new NormalDiceRollCommand(commandText);
                command.Execute();

                command.RollResult.ShouldBe(1);
            }
        }

        [TestMethod]
        public void ExecuteRollsA1d20AlwaysRollsBetween1And20()
        {
            var commandText = "1d20";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                var command = new NormalDiceRollCommand(commandText);
                command.Execute();

                allResults.Add(command.RollResult);

                command.RollResult.ShouldBeGreaterThanOrEqualTo(1);
                command.RollResult.ShouldBeLessThanOrEqualTo(20);
            }

            for(int i = 1; i <= 20; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void ExecuteRollsA5d5AlwaysRollsBetween5And25()
        {
            var commandText = "5d5";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                var command = new NormalDiceRollCommand(commandText);
                command.Execute();

                allResults.Add(command.RollResult);

                command.RollResult.ShouldBeGreaterThanOrEqualTo(5);
                command.RollResult.ShouldBeLessThanOrEqualTo(25);
            }

            for (int i = 5; i <= 25; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void ExecuteRollsA1d20Plus5AlwaysRollsBetween6And25()
        {
            var commandText = "1d20+5";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                var command = new NormalDiceRollCommand(commandText);
                command.Execute();

                allResults.Add(command.RollResult);

                command.RollResult.ShouldBeGreaterThanOrEqualTo(6);
                command.RollResult.ShouldBeLessThanOrEqualTo(25);
            }

            for (int i = 6; i <= 25; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void ExecuteRollsA1d20Minus5AlwaysRollsBetweenMinus4And15()
        {
            var commandText = "1d20-5";

            var allResults = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                var command = new NormalDiceRollCommand(commandText);
                command.Execute();

                allResults.Add(command.RollResult);

                command.RollResult.ShouldBeGreaterThanOrEqualTo(-4);
                command.RollResult.ShouldBeLessThanOrEqualTo(15);
            }

            for (int i = -4; i <= 15; i++)
            {
                allResults.ShouldContain(i);
            }
        }

        [TestMethod]
        public void ExecuteShouldCheckCommandFormatOnWrongFormatThrowFormatException()
        {
            var commandText = "1dd20";

            var command = new NormalDiceRollCommand(commandText);

            var exception = Should.Throw<FormatException>(() => command.Execute());

            exception.Message.ShouldBe("The format was not correct.");
        }
    }
}
