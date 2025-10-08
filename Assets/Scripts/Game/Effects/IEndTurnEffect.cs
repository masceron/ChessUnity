namespace Game.Effects
{
    public enum EndTurnEffectType : byte
    {
        EndOfAllyTurn, EndOfEnemyTurn, EndOfAnyTurn
    }
    
    //Interface for triggers that took place on a new turn.
    //There are two types of end turn trigger:
    //Those fire on the start of the opposite side's turn, and those fire on the start of its side's turn.
    public interface IEndTurnEffect
    {
        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction);
    }
}