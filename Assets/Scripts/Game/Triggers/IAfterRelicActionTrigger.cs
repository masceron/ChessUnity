using System;
using Game.Action.Relics;

namespace Game.Triggers
{
    public interface IAfterRelicActionTrigger : IComparable<IAfterRelicActionTrigger>
    {
        public AfterActionPriority Priority { get; }

        int IComparable<IAfterRelicActionTrigger>.CompareTo(IAfterRelicActionTrigger other)
        {
            return other == null ? 1 : ((int)other.Priority).CompareTo((int)Priority);
        }

        public void OnCallAfterRelicAction(IRelicAction relicAction);
    }
}