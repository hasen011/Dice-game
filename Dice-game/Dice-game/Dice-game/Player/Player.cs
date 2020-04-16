using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_game.PlayerDomain
{
    public class Player
    {
        private Random Random { get; set; }
        public short[] CurrentDice { get; set; }

        // Determines what dice a player decide to keep and not re-roll
        public bool[] FixedDice { get; set; }
        public short NumberOfRolls { get; set; }
        public PlayerType PlayerType { get; set; }

        public Player(PlayerType playerType)
        {
            PlayerType = playerType;
            Random = new Random();
            CurrentDice = new short[6];
            FixedDice = Enumerable.Repeat(false, 6).ToArray();
            NumberOfRolls = 2;
        }

        public void RollDice()
        {
            for (var i = 0; i < FixedDice.Length; i++)
            {
                if (!FixedDice[i])
                {
                    CurrentDice[i] = (short)Random.Next(1, 7);
                }
            }

            DisplayDice();
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
                case PlayerAction.Yield:
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
            Console.WriteLine("    3. Yield.");
        }
    }
}
