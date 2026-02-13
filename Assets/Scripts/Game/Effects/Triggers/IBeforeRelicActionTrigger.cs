using System;
using Game.Action.Relics;

namespace Game.Effects.Triggers
{
    public interface IBeforeRelicActionTrigger: IComparable<IBeforeRelicActionTrigger>
    {
        public BeforeActionPriority Priority { get; }
        public void OnCallBeforeRelicAction(IRelicAction relicAction);

        int IComparable<IBeforeRelicActionTrigger>.CompareTo(IBeforeRelicActionTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }
    }
}