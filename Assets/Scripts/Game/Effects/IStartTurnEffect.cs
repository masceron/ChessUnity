namespace Game.Effects
{
    public enum StartTurnEffectType : byte
    {
        StartOfAllyTurn, StartOfEnemyTurn, StartOfAnyTurn
    }
    
    /// <summary>
    /// <para>Do NOT use on the same class that implements IEndTurnEffect.</para>
    /// <para>The equivalent of IEndturnEffect on the start of turns.</para>
    /// </summary>
    public interface IStartTurnEffect
    {
        public StartTurnEffectType StartTurnEffectType { get; }
        
        public void OnCallStart(Action.Action lastMainAction);
    }
}