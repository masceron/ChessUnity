using System;
using Game.Action.Relics;

namespace Game.Effects.Triggers
{
    public interface IAfterRelicActionTrigger: IComparable<IAfterRelicActionTrigger>
    {
        public AfterActionPriority Priority { get; }
        public void OnCallAfterRelicAction(IRelicAction relicAction);
        
        int IComparable<IAfterRelicActionTrigger>.CompareTo(IAfterRelicActionTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }
    }
}