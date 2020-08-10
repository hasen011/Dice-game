using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using Dice_game.PlayerDomain.Utility;
using System;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class Game
    {
        public Player[] Players { get; set; }
        public int PlayerToPlay { get; set; }
        public int? PlayerToEndRound { get; set; }
        public Round Round { get; set; }
        public bool EndGame { get; set; }

        public Game(Player[] players)
        {
            Players = players;
            Round = Round.One;
        }

        public Game(Player[] players, Round round)
        {
            Players = players;
            Round = round;
        }

        public void StartGame()
        {
            // The game consists of three rounds where players try to get as many combinations for as many points as possible.
            // The three rounds have slightly different rules.
            // Round One - players can end their turn as many times as they want and save their unsued rolls for future rolls.
            //             Also, they can decide to play any combinations at any time, i.e. they can go for doubles but if they roll
            //             which get them closer to triples then they can play triples. Round ends when any player finishes the board.
            //             Remaining players can finish their last turns.
            //             Note - this round can have as many turns as needed.
            //
            // Round Two - players need to determine what combination (regardless of the dice values) they are trying to get after the first
            //             roll in each turn. For example, afterthe first roll, the player determines that the will go for sixes.
            //             If they don't get sixes in their available rolls or they decide to give up and end their their turn,
            //             sixes are crossed out for them and they cannot score any points for sixes during this round.
            //             Note - this round will have as many turns as there are combinations.
            //
            // Round Three - the board is played from the top to the bottom, i.e. first round each players needs to complete ones,
            //               second round twos, etc. Any combination they don't complete, or if they decide to end their turn, is crossed out
            //               and players cannot score any points for that combination in this round.
            //               Note - this round will have as many turns as there are combinations.
            //
            // Any unused rolls are turned into points (one for one) at the end of each round.

            PlayerToPlay = 0;
            PlayerToEndRound = null;
            EndGame = false;

            // Set turn for players
            foreach (var p in Players)
            {
                p.Round = Round;
            }

            Console.WriteLine($"Player's {PlayerToPlay} turn.");
            Players[PlayerToPlay].TakeTurn();

            while (1 == 1)
            {
                if (EndGame)
                {
                    Console.WriteLine($"Ending game...");
                    break;
                }

                if (PlayerToEndRound == PlayerToPlay)
                {
                    if (Round == Round.Three)
                    {
                        // Game end
                        for (var i = 0; i < Players.Length; i++)
                        {
                            Console.WriteLine($"Player's {i} score: {Players[i].Board.TotalScore}");
                        }

                        Console.WriteLine($"Player(s) {string.Join(", ", Players.Where(p => p.Board.TotalScore == Players.Max(p => p.Board.TotalScore)).Select(p => p.Name))} win(s).");

                        break;
                    }
                    else
                    {
                        // Round end
                        for (var i = 0; i < Players.Length; i++)
                        {
                            Console.WriteLine($"Player's {i} score: {Players[i].Board.TotalScore}");
                        }

                        PlayerToEndRound = null;
                        Round += 1; // Next round - this works because round is an enum
                                    // Set turn for players
                        foreach (var p in Players)
                        {
                            p.Round = Round;
                        }
                    }
                }

                ResolveAction(Players[PlayerToPlay].NextAction());
            }
        }

        public void NextTurn()
        {
            if (IsBoardCompleted(PlayerToPlay) && PlayerToEndRound == null)
            {
                PlayerToEndRound = PlayerToPlay;
            }

            PlayerToPlay = ++PlayerToPlay % Players.Length;
            Console.WriteLine($"Player's {PlayerToPlay} turn.");
            Players[PlayerToPlay].TakeTurn();
        }

        public void ResolveAction(PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.EndTurn:
                case PlayerAction.AssignCombination:
                    NextTurn();
                    break;

                case PlayerAction.EndGame:
                    EndGame = true;
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
