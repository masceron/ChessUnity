using System;
using Game.Action.Relics;

namespace Game.Triggers
{
    public interface IBeforeRelicActionTrigger : IComparable<IBeforeRelicActionTrigger>
    {
        public BeforeActionPriority Priority { get; }

        int IComparable<IBeforeRelicActionTrigger>.CompareTo(IBeforeRelicActionTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }

        public void OnCallBeforeRelicAction(IRelicAction relicAction);
    }
}