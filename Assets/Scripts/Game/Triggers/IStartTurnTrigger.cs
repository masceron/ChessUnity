using System;

namespace Game.Triggers
{
    public enum StartTurnEffectType : byte
    {
        StartOfAllyTurn,
        StartOfEnemyTurn,
        StartOfAnyTurn
    }

    public enum StartTurnTriggerPriority
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
    ///     <para>Do NOT use on the same class that implements IEndTurnTrigger.</para>
    ///     <para>The equivalent of IEndturnEffect on the start of turns.</para>
    /// </summary>
    public interface IStartTurnTrigger : IComparable<IStartTurnTrigger>
    {
        public StartTurnTriggerPriority Priority { get; }
        public StartTurnEffectType StartTurnEffectType { get; }

        int IComparable<IStartTurnTrigger>.CompareTo(IStartTurnTrigger other)
        {
            return ((int)other.Priority).CompareTo((int)Priority);
        }

        public void OnCallStart(Action.Action lastMainAction);
    }
}