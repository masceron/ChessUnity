using System;

namespace Game.Effects.Triggers
{
    public enum AfterActionPriority
    {
        Move = 100,
        Kill = 90,
        Debuff = 80,
        Buff = 70,
        Other = 60
    }

    public interface IAfterPieceActionTrigger : IComparable<IAfterPieceActionTrigger>
    {
        public AfterActionPriority Priority { get; }

        int IComparable<IAfterPieceActionTrigger>.CompareTo(IAfterPieceActionTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }

        public void OnCallAfterPieceAction(Action.Action action);
    }
}