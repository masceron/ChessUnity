using System;
using Game.Action.Internal;

namespace Game.Triggers
{
    public enum BeforeApplyEffectTriggerPriority
    {
        Prevention = 100,
        Survival = 90,
        Reaction = 80
    }

    public interface IBeforeApplyEffectTrigger : IComparable<IBeforeApplyEffectTrigger>
    {
        public BeforeApplyEffectTriggerPriority Priority { get; }

        int IComparable<IBeforeApplyEffectTrigger>.CompareTo(IBeforeApplyEffectTrigger other)
        {
            return ((int)other.Priority).CompareTo((int)Priority);
        }

        void OnCallApplyEffect(ApplyEffect applyEffect);
    }
}