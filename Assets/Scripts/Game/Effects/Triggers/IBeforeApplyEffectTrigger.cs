using System;
using Game.Action.Internal;

namespace Game.Effects.Triggers
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
            return other.Priority.CompareTo(Priority);
        }

        void OnCallApplyEffect(ApplyEffect applyEffect);
    }
}