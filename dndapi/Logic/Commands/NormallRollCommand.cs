using Database;
using System;

namespace Logic.Commands
{
    public class NormallRollCommand : Command
    {
        public NormallRollCommand(string arguments) : base(arguments)
        {
        }

        public override CommandResult Run()
        {
            var rollResult = new NormalRollCommandResult();

            (int firstNumber, int secondNumber, string[] expression, char @operator) = ReadArguments();

            int roll = RollDice(firstNumber, secondNumber);

            if (expression?.Length > 1)
            {
                roll = CalculateExpression(expression, roll, @operator);
            }

            rollResult.Result = roll;

            return rollResult;
        }

        private static int RollDice(int firstNumber, int secondNumber)
        {
            int roll = 0;
            
            for (int i = 0; i < firstNumber; i++)
            {
                var random = new Random();
                roll = roll + random.Next(1, secondNumber + 1);
            }

            return roll;
        }

        private (int firstNumber, int secondNumber, string[] expression, char @operator) ReadArguments()
        {
            var arguments = GetArguments();
            arguments = arguments.Replace(" ", "");
            var splitArguments = arguments.Split('d');

            var firstNumber = int.Parse(splitArguments[0]);
            var secondNumber = 0;
            string[] expression = null;
            var @operator = ' ';

            if (ContainsOperator(splitArguments[1]))
            {
                @operator = GetOperator(splitArguments[1]);
                expression = splitArguments[1].Split(@operator);

                secondNumber = int.Parse(expression[0]);
            }
            else
            {
                secondNumber = int.Parse(splitArguments[1]);
            }

            return (firstNumber, secondNumber, expression, @operator);

        }

        private char GetOperator(string expression)
        {
            if (expression.Contains('+'))
            {
                return '+';
            }
            else if (expression.Contains('-'))
            {
                return '-';
            }

            return ' ';
        }

        private int CalculateExpression(string[] expression, int rollResult, char @operator)
        {
            var expressionSecondNumber = int.Parse(expression[1]);

            if (@operator == '+')
            {
                return rollResult + expressionSecondNumber;
            }
            else if (@operator == '-')
            {
                return rollResult - expressionSecondNumber;
            }

            return rollResult;
        }

        private bool ContainsOperator(string argument)
        {
            if (argument.Contains('+') || argument.Contains('-'))
            {
                return true;
            }

            return false;
        }
    }
}
