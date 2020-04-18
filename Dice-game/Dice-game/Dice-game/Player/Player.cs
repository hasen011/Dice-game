using Dice_game.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_game.PlayerDomain
{
    public class Player
    {
        public Random Random { get; set; }
        public int[] CurrentDice { get; set; }
        public Combination[] CurrentMatchingCombinations { get; set; }

        // Determines what dice a player decide to keep and not re-roll
        public bool[] FixedDice { get; set; }
        public int NumberOfRollsInCurrentRound { get; set; }
        public int TotalNumberOfRolls { get; set; }
        public PlayerType PlayerType { get; set; }

        // Infrastruce objects
        public Board Board { get; set; }
        public CombinationList CombinationList { get; set; }

        public Player(PlayerType playerType)
        {
            PlayerType = playerType;
            Random = new Random();
            CurrentDice = new int[6];
            FixedDice = Enumerable.Repeat(false, 6).ToArray();
            TotalNumberOfRolls = 0;
            Board = new Board();
            CombinationList = new CombinationList();
        }

        // Methods
        public void RollDice()
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

        public void FixDice(string indexes)
        {
            FixedDice = Enumerable.Repeat(false, 6).ToArray();

            for (var i = 0; i < indexes.Length; i++)
            {
                FixedDice[indexes[i] - '0'] = true;
            }

            DisplayDice();
        }

        public void AssignCombination(string combinationIndex)
        {
            var combination = CurrentMatchingCombinations[int.Parse(combinationIndex)];

            if (!Board.CurrentBoard[combination.CombinationType].Completed)
            {
                combination.Completed = true;
                Board.CurrentBoard[combination.CombinationType] = combination;
            }
            else
            {
                Console.WriteLine("This combination has already been completed.");
            }
        }

        // TODO: Add a concept of a round
        public void NextAction()
        {
            // Round always starts with a roll, then player has 2 + 'any previously saved rolls' rolls
            // If a player doesn't use the two given rolls (one or both), they can yield and save the remaining
            // roll(s) for next rounds
            // Round ends either by yielding, assigning a combination or running out of rolls
            RollDice();
            //NumberOfRollsInCurrentRound = 2 + TotalNumberOfRolls;
            DisplayDice();

            DisplayActionList();
            Console.Write("What action do you want to take? ");
            Enum.TryParse(Console.ReadLine(), out PlayerAction action);

            switch (action)
            {
                case PlayerAction.RollDice:
                    RollDice();
                    NumberOfRollsInCurrentRound--;
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
                    TotalNumberOfRolls += NumberOfRollsInCurrentRound;
                    break;

                case PlayerAction.ShowBoard:
                    ShowBoard();
                    break;

                case PlayerAction.Yield:
                    TotalNumberOfRolls += NumberOfRollsInCurrentRound;
                    break;

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
    }
}
