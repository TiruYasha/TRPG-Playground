using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Domain.Commands
{
    public class NormalDiceRollCommand : Command
    {
        public int RollResult { get; set; }
        public NormalDiceRollCommand(string commandText) : base(CommandType.NormallDiceRoll, commandText)
        {

        }

        public override void Execute()
        {
            CheckCommandText();
            (int amountOfDice, int totalDieValue, char @operator, int number) = ParseCommandText();

            int roll = RollDice(amountOfDice, totalDieValue);

            if (@operator != ' ')
            {
                roll = CalculateExpression(roll, @operator, number);
            }

            RollResult = roll;
        }

        private void CheckCommandText()
        {
            var regexPattern = @"^[1-9][0-9]*[dD][1-9][0-9]*([+-][1-9]{1})?$";
            if(!Regex.IsMatch(CommandText, regexPattern))
            {
                throw new FormatException("The format was not correct.");
            }
        }

        private int CalculateExpression(int roll, char @operator, int number)
        {
            if(@operator == '+')
            {
                return roll + number;
            }
            else
            {
                return roll - number;
            }
        }

        private (int amountOfDice, int totalDieValue, char @operator, int number) ParseCommandText()
        {
            var splittedCommandText = CommandText.Split(new char[] { 'd', 'D' }, StringSplitOptions.RemoveEmptyEntries);

            int.TryParse(splittedCommandText[0], out int amountOfDice);
            
            if (ContainsOperator(splittedCommandText[1]))
            {
                (int totalDieValueExpression, char @operator, int number) = ParseExpression(splittedCommandText[1]);

                return (amountOfDice, totalDieValueExpression, @operator, number);
            }

            int.TryParse(splittedCommandText[1], out int totalDieValue);

            return (amountOfDice, totalDieValue, ' ', 0);
        }

        private (int totalDieValue, char @operator, int number) ParseExpression(string expression)
        {
            var @operator = expression.Contains('+') ? '+' : '-';
            var splittedExpression = expression.Split(@operator, StringSplitOptions.RemoveEmptyEntries);

            int.TryParse(splittedExpression[0], out int totalDieValue);
            int.TryParse(splittedExpression[1], out int number);

            return (totalDieValue, @operator, number);
            
        }

        private bool ContainsOperator(string expression)
        {
            return expression.Contains('+') || expression.Contains('-');
        }

        private int RollDice(int amountOfDice, int totalDieValue)
        {
            int roll = 0;

            for (int i = 0; i < amountOfDice; i++)
            {
                var random = new Random();
                roll = roll + random.Next(1, totalDieValue + 1);
            }

            return roll;
        }
    }
}
