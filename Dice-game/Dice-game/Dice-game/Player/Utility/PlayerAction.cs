namespace Dice_game.PlayerDomain.Utility
{
    public enum PlayerAction
    {
        InvalidAction = 0,
        RollDice,
        FixDice,
        AssignCombination,       
        ShowBoard,
        EndTurn,
        EndGame,
        EvaluateAllCombinations,

        PickCombinationToPlay = 98, // In second round, players must pick combinations to play after the first roll
        AssignDice = 99, // This is used to manually override dice values
    }
}
