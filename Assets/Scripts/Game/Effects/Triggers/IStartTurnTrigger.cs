using System;

namespace Game.Effects.Triggers
{
    public enum StartTurnEffectType : byte
    {
        StartOfAllyTurn, StartOfEnemyTurn, StartOfAnyTurn
    }
    
    public enum StartTurnTriggerPriority
    {
        Move = 100,
        Kill = 90,
        Debuff = 80,
        Buff = 70,
        Other = 60,
    }
    
    /// <summary>
    /// <para>Do NOT use on the same class that implements IEndTurnTrigger.</para>
    /// <para>The equivalent of IEndturnEffect on the start of turns.</para>
    /// </summary>
    public interface IStartTurnTrigger: IComparable<IStartTurnTrigger>
    {
        public StartTurnTriggerPriority Priority { get; }
        public StartTurnEffectType StartTurnEffectType { get; }
        
        public void OnCallStart(Action.Action lastMainAction);

        int IComparable<IStartTurnTrigger>.CompareTo(IStartTurnTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }
    }
}