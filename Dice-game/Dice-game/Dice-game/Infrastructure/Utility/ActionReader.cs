using Dice_game.PlayerDomain.Utility;
using System;
using System.Linq;

namespace Dice_game.Infrastructure.Utility
{
    public class ActionReader
    {
        // Expected format is action followed by optional parameters.
        // Example: 1 2,3 (roll dice 2 and 3)
        public virtual (PlayerAction action, int[] param) GetNextAction(Round round, bool firstAction)
        {
            var input = Console.ReadLine();

            // If it's Round.Two and first action, force player to pick a combination to play
            if (round == Round.Two && firstAction)
            {
                input = $"{PlayerAction.PickCombinationToPlay} " + input;
            }
            
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
                if (action == PlayerAction.FixDice
                    || action == PlayerAction.AssignDice
                    || action == PlayerAction.AssignCombination
                    || action == PlayerAction.PickCombinationToPlay)
                {
                    return (PlayerAction.InvalidAction, new int[0]);
                }

                return (action, new int[0]);
            }
            else if (temp.Length == 2)
            {
                var param = temp[1].ToCharArray().Select(x => x - '0').ToArray();
                
                // Checks
                if (param.Any(p => p < 0) || param.Any(p => p > 6))
                {
                    return (PlayerAction.InvalidAction, new int[0]);
                }
                else if (action == PlayerAction.AssignCombination)
                {
                    if (param.Length != 1)
                    {
                        return (PlayerAction.InvalidAction, new int[0]);
                    }                
                }
                else if (action == PlayerAction.FixDice)
                {
                    if (param.Any(p => p < 0 || p > 5) || param.Length > 6)
                    {
                        return (PlayerAction.InvalidAction, new int[0]);
                    }                
                }
                else if (action == PlayerAction.AssignDice)
                {
                    if (param.Any(p => p < 1 || p > 6) || param.Length != 6)
                    {
                        return (PlayerAction.InvalidAction, new int[0]);
                    }                 
                }
                else if (action == PlayerAction.PickCombinationToPlay)
                {
                    if (param.Length != 1 || !Enum.IsDefined(typeof(CombinationType), param[0]))
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
        public override (PlayerAction action, int[] param) GetNextAction(Round round, bool firstAction)
        {
            var input = InputSequence[index++];

            // If it's Round.Two and first action, force player to pick a combination to play
            if (round == Round.Two && firstAction)
            {
                input = $"{PlayerAction.PickCombinationToPlay} " + input;
            }

            return ParseInput(input);
        }
    }
}
