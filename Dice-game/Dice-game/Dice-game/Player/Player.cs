using Dice_game.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Dice_game.PlayerDomain
{
    public class Player
    {
        private Random Random { get; set; }
        public int[] CurrentDice { get; set; }
        public Combination[] CurrentPossibleCombinations { get; set; }

        // Determines what dice a player decide to keep and not re-roll
        public bool[] FixedDice { get; set; }
        public int TotalNumberOfRolls { get; set; }
        public PlayerType PlayerType { get; set; }

        // Infrastruce objects
        public Board Board { get; set; }
        public CombinationList CombinationList { get; set; }
        public ActionReader ActionReader { get; set; }

        public Player(PlayerType playerType)
        {
            PlayerType = playerType;
            Random = new Random();
            CurrentDice = new int[6];
            FixedDice = Enumerable.Repeat(false, 6).ToArray();
            TotalNumberOfRolls = 0;
            Board = new Board();
            CombinationList = new CombinationList();
            ActionReader = new ActionReader();
        }

        public Player(PlayerType playerType, ActionReader actionReader)
        {
            PlayerType = playerType;
            Random = new Random();
            CurrentDice = new int[6];
            FixedDice = Enumerable.Repeat(false, 6).ToArray();
            TotalNumberOfRolls = 0;
            Board = new Board();
            CombinationList = new CombinationList();
            ActionReader = actionReader;
        }

        // Methods
        public void RollDice(bool sortDiceAfterRoll = false)
        {
            if (TotalNumberOfRolls == 0)
            {
                Console.WriteLine("No rolls left! Assign a combination or yield!");
                return;
            }

            for (var i = 0; i < FixedDice.Length; i++)
            {
                if (!FixedDice[i])
                {
                    CurrentDice[i] = Random.Next(1, 7);
                }
            }

            // Should we sort the array after every roll?
            // Sort FixedDice according to how CurrentDice is sorted
            if (sortDiceAfterRoll)
            {
                Array.Sort(CurrentDice, FixedDice);
            }         

            TotalNumberOfRolls--;

            EvaluateDice();
            DisplayDice();
            DisplayPossibleCombinations(CurrentPossibleCombinations);         
        }

        public void FixDice(int[] indexes)
        {
            ResetFixedDice();

            for (var i = 0; i < indexes.Length; i++)
            {
                FixedDice[indexes[i]] = true;
            }

            DisplayDice();
        }

        public bool TryAssignCombination(int combinationIndex)
        {
            if (combinationIndex < 0 || combinationIndex >= CurrentPossibleCombinations.Length)
            {
                Console.WriteLine("Pick a valid combination.");
                return false;
            }

            var combination = CurrentPossibleCombinations[combinationIndex];

            if (!Board.CurrentBoard[combination.CombinationType].Completed)
            {
                combination.Completed = true;
                Board.CurrentBoard[combination.CombinationType] = combination;
                Board.TotalScore += combination.Score;

                return true;
            }
            else
            {
                Console.WriteLine("This combination has already been completed.");
                return false;
            }
        }

        public void TakeTurn()
        {
            ResetFixedDice();

            // Round always starts with a roll, then player has 2 + 'any previously saved rolls' rolls
            // If a player doesn't use the two given rolls (one or both), they can yield and save the remaining
            // roll(s) for next rounds
            TotalNumberOfRolls += 3;
            RollDice(true); // This will subtract a roll, that's why we add 3 above
        }

        public PlayerAction NextAction()
        {
            
            // Round ends either by yielding, assigning a combination or running out of rolls
            DisplayActionList();
            Console.Write($"What action do you want to take ({TotalNumberOfRolls} rolls left)? ");
            var (action, param) = ActionReader.GetNextAction();

            switch (action)
            {
                case PlayerAction.RollDice:
                    RollDice(true);
                    return action;

                case PlayerAction.FixDice:
                    FixDice(param);
                    return action;

                case PlayerAction.AssignCombination:
                    if (TryAssignCombination(param[0]))
                    {
                        return action;
                    }
                    else
                    {
                        return PlayerAction.InvalidAction;
                    }                  

                case PlayerAction.ShowBoard:
                    ShowBoard();
                    return action;

                case PlayerAction.Yield:
                    return action;

                case PlayerAction.InvalidAction:
                    Console.WriteLine("Invalid action!");
                    return action;

                case PlayerAction.EndGame:
                    return action;

                // Action for testing purposes
                case PlayerAction.AssignDice:
                    CurrentDice = param;
                    EvaluateDice();
                    DisplayDice();
                    DisplayPossibleCombinations(CurrentPossibleCombinations);
                    return action;

                default:
                    throw new Exception();
            }
        }

        public void DisplayDice()
        {
            for (var i = 0; i < CurrentDice.Length; i++)
            {
                Console.Write(CurrentDice[i]);
                if (FixedDice[i])
                {
                    Console.Write("x");
                }
                if (i != 5)
                {
                    Console.Write(", ");
                }
                
            }
            Console.WriteLine();
        }

        public void DisplayActionList()
        {
            Console.WriteLine("Actions: ");
            Console.WriteLine("    1. Roll dice.");
            Console.WriteLine("    2. Fix dice.");
            Console.WriteLine("    3. Assign combination.");
            Console.WriteLine("    4. Show board.");
            Console.WriteLine("    5. Yield.");
            Console.WriteLine("    6. EndGame.");
        }

        public void DisplayPossibleCombinations(Combination[] matchingCombinations)
        {
            Console.WriteLine("PossibleCombinations: ");

            for (var i = 0; i < matchingCombinations.Length; i++)
            {
                Console.WriteLine($"     {i}. {matchingCombinations[i].CombinationType} - {string.Join(",", matchingCombinations[i].Dice)}");
            }
        }

        public void ShowBoard()
        {
            foreach (var combination in Board.CurrentBoard)
            {
                Console.WriteLine($"{combination.Value.CombinationType}: {combination.Value.Completed} - {combination.Value.Score}");
            }
        }

        public void EvaluateDice()
        {
            // Find possible combinations. Don't include combinations which were already completed
            var matchingCombinations = CombinationList.LookupMatchingCombinations(CurrentDice);
            var tempCombinations = new List<Combination>();
            foreach (var combination in matchingCombinations)
            {
                if (!Board.CurrentBoard[combination.CombinationType].Completed)
                {
                    tempCombinations.Add(combination);
                }
            }
            CurrentPossibleCombinations = tempCombinations.ToArray();
        }

        private void ResetFixedDice()
        {
            for (var i = 0; i < FixedDice.Length; i++)
            {
                FixedDice[i] = false;
            }
        }



    }
}
