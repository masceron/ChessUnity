using System;
using Game.Action.Relics;

namespace Game.Triggers
{
    public interface IAfterRelicActionTrigger : IComparable<IAfterRelicActionTrigger>
    {
        public AfterActionPriority Priority { get; }

        int IComparable<IAfterRelicActionTrigger>.CompareTo(IAfterRelicActionTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }

        public void OnCallAfterRelicAction(IRelicAction relicAction);
    }
}