using System;

namespace Game.Effects.Triggers
{
    public enum EndTurnEffectType : byte
    {
        EndOfAllyTurn, EndOfEnemyTurn, EndOfAnyTurn
    }

    public enum EndTurnTriggerPriority
    {
        Move = 100,
        Kill = 90,
        Debuff = 80,
        Buff = 70,
        Other = 60,
    }

    /// <summary>
    /// <para>
    /// Interface for triggers that took place on a new turn.
    /// There are two types of end turn trigger:
    /// Those fire on the start of the opposite side's turn, and those fire on the start of its side's turn.
    /// </para>
    /// <para>Do NOT use on the same class that implements IStartTurnTrigger.</para>
    /// </summary>
    public interface IEndTurnTrigger: IComparable<IEndTurnTrigger>
    {
        public EndTurnTriggerPriority Priority { get; }
        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction);

        int IComparable<IEndTurnTrigger>.CompareTo(IEndTurnTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }
    }
    
}