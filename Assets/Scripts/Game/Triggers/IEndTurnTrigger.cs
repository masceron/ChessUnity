using System;

namespace Game.Triggers
{
    public enum EndTurnEffectType : byte
    {
        EndOfAllyTurn,
        EndOfEnemyTurn,
        EndOfAnyTurn
    }

    public enum EndTurnTriggerPriority
    {
        FormationMove = 200,
        FormationKill = 190,
        FormationDebuff = 180,
        FormationBuff = 170,
        FormationOther = 160,
        Move = 100,
        Kill = 90,
        Debuff = 80,
        Buff = 70,
        Other = 60
    }

    /// <summary>
    ///     <para>
    ///         Interface for triggers that took place on a new turn.
    ///         There are two types of end turn trigger:
    ///         Those fire on the start of the opposite side's turn, and those fire on the start of its side's turn.
    ///     </para>
    ///     <para>Do NOT use on the same class that implements IStartTurnTrigger.</para>
    /// </summary>
    public interface IEndTurnTrigger : IComparable<IEndTurnTrigger>
    {
        public EndTurnTriggerPriority Priority { get; }
        public EndTurnEffectType EndTurnEffectType { get; }

        int IComparable<IEndTurnTrigger>.CompareTo(IEndTurnTrigger other)
        {
            return ((int)other.Priority).CompareTo((int)Priority);
        }

        public void OnCallEnd(Action.Action lastMainAction);
    }
}