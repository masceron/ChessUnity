using System;

namespace Game.Effects.Triggers
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