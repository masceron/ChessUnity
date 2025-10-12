namespace Game.Effects
{
    public enum StartTurnEffectType : byte
    {
        StartOfAllyTurn, StartOfEnemyTurn, StartOfAnyTurn
    }
    
    //Interface for triggers that took place on a new turn.
    //There are two types of end turn trigger:
    //Those fire on the start of the opposite side's turn, and those fire on the start of its side's turn.
    public interface IStartTurnEffect
    {
        public StartTurnEffectType StartTurnEffectType { get; }
        public void OnCallStart(Action.Action startMainAction);
    }
}