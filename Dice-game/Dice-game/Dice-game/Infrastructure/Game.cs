﻿using Dice_game.PlayerDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_game.Infrastructure
{
    public class Game
    {
        public Player[] Players { get; set; }
        public int PlayerToPlay { get; set; }
        public int Round { get; set; }
        public bool LastRound { get; set; }

        public Game(Player[] players)
        {
            Players = players;
        }

        public void StartGame()
        {
            Round = 1;
            PlayerToPlay = 0;
            LastRound = false;

            Console.WriteLine($"Player's {PlayerToPlay} turn.");
            Players[PlayerToPlay].TakeTurn();

            while (1 == 1)
            {
                var nextAction = Players[PlayerToPlay].NextAction();
                if (nextAction == PlayerAction.Yield
                    || nextAction == PlayerAction.AssignCombination)
                {
                    NextTurn();
                }

                if (PlayerToPlay == 0 && LastRound)
                {
                    EndGame();
                    break;
                }
            }
        }

        public void EndGame()
        {
            for (var i = 0; i < Players.Length; i++)
            {
                Console.WriteLine($"Player's {i} score: {Players[i].Board.TotalScore}");
            }

            Console.WriteLine($"Player(s) {string.Join(", ", Players.Where(p => p.Board.TotalScore == Players.Max(p => p.Board.TotalScore)))} win(s).");
        }

        public void NextTurn()
        {
            if (IsBoardCompleted(PlayerToPlay))
            {
                LastRound = true;
            }

            PlayerToPlay = ++PlayerToPlay % Players.Length;
            Console.WriteLine($"Player's {PlayerToPlay} turn.");
            Players[PlayerToPlay].TakeTurn();
        }

        public void ResolveAction(PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.Yield:              
                    NextTurn();
                    break;

                default:
                    break;
            }
        }

        public bool IsBoardCompleted(int playerToPlay)
        {
            if (Players[playerToPlay].Board.CurrentBoard.Any(c => !c.Value.Completed))
            {
                return false;
            }

            return true;
        }
    }
}
