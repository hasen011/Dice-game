using Dice_game.PlayerDomain;
using System;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class ActionReader
    {
        // Expected format is action followed by optional parameters.
        // Example: 1 2,3 (roll dice 2 and 3)
        public virtual (PlayerAction action, int[] param) GetNextAction()
        {
            var input = Console.ReadLine();
            
            return ParseInput(input);
        }

        // TODO: Add more input sanitization
        public (PlayerAction action, int[] param) ParseInput(string input)
        {
            if (input.Length == 0)
            {
                return (PlayerAction.InvalidAction, new int[0]);
            }

            var temp = input.Split(" ");
            Enum.TryParse(temp[0], out PlayerAction action);
            // Check if the action value is defined
            if (!Enum.IsDefined(typeof(PlayerAction), action))
            {
                return (PlayerAction.InvalidAction, new int[0]);
            }

            if (temp.Length == 1)
            {            
                return (action, new int[0]);
            }
            else if (temp.Length == 2)
            {
                var p = temp[1];
                var param = p.Split(",").Select(d => int.Parse(d)).ToArray();

                // Checks
                if (action == PlayerAction.AssignCombination && param.Length != 1)
                {
                    return (PlayerAction.InvalidAction, new int[0]);
                }

                if (action == PlayerAction.FixDice || action == PlayerAction.AssignDice)
                {
                    if (param.Length < 1 || param.Length > 6)
                    {
                        return (PlayerAction.InvalidAction, new int[0]);
                    }
                }

                return (action, param);
            }
            else
            {
                return (PlayerAction.InvalidAction, new int[0]);
            }
        }
    }

    public class ActionReaderSequence : ActionReader
    {
        private int index = 0;
        private string[] InputSequence { get; set; }

        public ActionReaderSequence(string[] inputSequence)
        {
            InputSequence = inputSequence;
        }
        public override (PlayerAction action, int[] param) GetNextAction()
        {
            return ParseInput(InputSequence[index++]);
        }
    }
}
