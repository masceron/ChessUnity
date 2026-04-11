using System;

namespace Game.Triggers
{
    public enum AfterActionPriority
    {
        Formation = 200,
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
            return other == null ? 1 : ((int)other.Priority).CompareTo((int)Priority);
        }

        public void OnCallAfterPieceAction(Action.Action action);
    }
}