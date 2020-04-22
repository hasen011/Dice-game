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
        public Random Random { get; set; }
        public int[] CurrentDice { get; set; }
        public Combination[] CurrentPossibleCombinations { get; set; }

        // Determines what dice a player decide to keep and not re-roll
        public bool[] FixedDice { get; set; }
        public int TotalNumberOfRolls { get; set; }
        public PlayerType PlayerType { get; set; }

        // Infrastruce objects
        public Board Board { get; set; }
        public CombinationList CombinationList { get; set; }
        public ReadInput ReadInput { get; set; }

        public Player(PlayerType playerType, ReadInput readInput)
        {
            PlayerType = playerType;
            Random = new Random();
            CurrentDice = new int[6];
            FixedDice = Enumerable.Repeat(false, 6).ToArray();
            TotalNumberOfRolls = 0;
            Board = new Board();
            CombinationList = new CombinationList();
            ReadInput = readInput;
        }

        // Methods
        public void RollDice()
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

            TotalNumberOfRolls--;

            EvaluateDice();
            DisplayDice();
            DisplayPossibleCombinations(CurrentPossibleCombinations);
        }

        public void FixDice(string indexes)
        {
            ResetFixedDice();

            for (var i = 0; i < indexes.Length; i++)
            {
                FixedDice[indexes[i] - '0'] = true;
            }

            DisplayDice();
        }

        public void AssignCombination(string combinationIndex)
        {
            var combination = CurrentPossibleCombinations[int.Parse(combinationIndex)];

            if (!Board.CurrentBoard[combination.CombinationType].Completed)
            {
                combination.Completed = true;
                Board.CurrentBoard[combination.CombinationType] = combination;
                Board.TotalScore += combination.Score;
            }
            else
            {
                Console.WriteLine("This combination has already been completed.");
            }
        }

        public void TakeTurn()
        {
            ResetFixedDice();

            // Round always starts with a roll, then player has 2 + 'any previously saved rolls' rolls
            // If a player doesn't use the two given rolls (one or both), they can yield and save the remaining
            // roll(s) for next rounds
            TotalNumberOfRolls += 3;
            RollDice(); // This will subtract a roll, that's why we add 3 above
        }

        public PlayerAction NextAction()
        {
            
            // Round ends either by yielding, assigning a combination or running out of rolls
            DisplayActionList();
            Console.Write($"What action do you want to take ({TotalNumberOfRolls} rolls left)? ");
            Enum.TryParse(ReadInput.GetNextInput(), out PlayerAction action);

            switch (action)
            {
                case PlayerAction.RollDice:
                    RollDice();
                    return action;

                case PlayerAction.FixDice:
                    Console.Write("Indexes of dice to fix: ");
                    var indexes = ReadInput.GetNextInput();
                    FixDice(indexes);
                    return action;

                case PlayerAction.AssignCombination:
                    Console.Write("Pick combination: ");
                    var combinationIndex = ReadInput.GetNextInput();
                    AssignCombination(combinationIndex);
                    return action;

                case PlayerAction.ShowBoard:
                    ShowBoard();
                    return action;

                case PlayerAction.Yield:
                    return action;

                case PlayerAction.InvalidAction:
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
            var matchingCombinations = CombinationList.LookupCombination(CurrentDice);
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
