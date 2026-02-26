using System;
using Game.Effects;

namespace Game.Triggers
{
    public interface IEffectStatModifierTrigger : IComparable<IEffectStatModifierTrigger>
    {
        int IComparable<IEffectStatModifierTrigger>.CompareTo(IEffectStatModifierTrigger other)
        {
            return 0;
        }

        public int Modify(EffectStat stat);
    }
}