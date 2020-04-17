using Dice_game.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_game.PlayerDomain
{
    public class Player
    {
        private Random Random { get; set; }
        private int[] CurrentDice { get; set; }
        private List<Combination> CurrentMatchingCombinations { get; set; }

        // Determines what dice a player decide to keep and not re-roll
        private bool[] FixedDice { get; set; }
        private int NumberOfRolls { get; set; }
        private PlayerType PlayerType { get; set; }

        // Infrastruce objects
        private Board Board { get; set; }
        private CombinationList CombinationList { get; set; }

        public Player(PlayerType playerType)
        {
            PlayerType = playerType;
            Random = new Random();
            CurrentDice = new int[6];
            FixedDice = Enumerable.Repeat(false, 6).ToArray();
            NumberOfRolls = 2;
            Board = new Board();
            CombinationList = new CombinationList();
        }

        // Methods
        private void RollDice()
        {
            for (var i = 0; i < FixedDice.Length; i++)
            {
                if (!FixedDice[i])
                {
                    CurrentDice[i] = Random.Next(1, 7);
                }
            }

            // Find possible combinations
            CurrentMatchingCombinations = CombinationList.LookupCombination(CurrentDice);

            DisplayDice();
            DisplayPossibleCombinations(CurrentMatchingCombinations);
        }

        private void FixDice(string indexes)
        {
            FixedDice = Enumerable.Repeat(false, 6).ToArray();

            for (var i = 0; i < indexes.Length; i++)
            {
                FixedDice[indexes[i] - '0'] = true;
            }

            DisplayDice();
        }

        private void AssignCombination(string combinationIndex)
        {
            var combination = CurrentMatchingCombinations[int.Parse(combinationIndex)];

            if (!Board.CurrentBoard[combination.CombinationType].Completed)
            {
                Board.CurrentBoard[combination.CombinationType] = combination;
            }
            else
            {
                Console.WriteLine("This combination has already been completed.");
            }
        }

        public void NextAction()
        {
            DisplayActionList();
            Console.Write("What action do you want to take? ");
            Enum.TryParse(Console.ReadLine(), out PlayerAction action);

            switch (action)
            {
                case PlayerAction.RollDice:
                    RollDice();
                    break;

                case PlayerAction.FixDice:
                    Console.Write("Indexes of dice to fix: ");
                    var indexes = Console.ReadLine();
                    FixDice(indexes);
                    break;

                case PlayerAction.AssignCombination:
                    Console.Write("Pick combination: ");
                    var combinationIndex = Console.ReadLine();
                    AssignCombination(combinationIndex);
                    break;

                case PlayerAction.ShowBoard:
                    ShowBoard();
                    break;

                case PlayerAction.Yield:
                    break;

                case PlayerAction.InvalidAction:
                default:
                    throw new Exception();
            }
        }

        private void DisplayDice()
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

        private void DisplayActionList()
        {
            Console.WriteLine("Actions: ");
            Console.WriteLine("    1. Roll dice.");
            Console.WriteLine("    2. Fix dice.");
            Console.WriteLine("    3. Assign combination.");
            Console.WriteLine("    4. Show board.");
            Console.WriteLine("    5. Yield.");
        }

        private void DisplayPossibleCombinations(List<Combination> matchingCombinations)
        {
            Console.WriteLine("PossibleCombinations: ");

            for (var i = 0; i < matchingCombinations.Count; i++)
            {
                Console.WriteLine($"     {i}. {matchingCombinations[i].CombinationType} - {string.Join(",", matchingCombinations[i].Dice)}");
            }
        }

        private void ShowBoard()
        {
            foreach (var combination in Board.CurrentBoard)
            {
                Console.WriteLine($"{combination.Value.CombinationType}: {combination.Value.Completed} - {combination.Value.Score}");
            }
        }
    }
}
