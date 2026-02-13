using System;

namespace Game.Effects.Triggers
{
    public interface IEffectStatModifierTrigger: IComparable<IEffectStatModifierTrigger>
    {
        public int Modify(EffectStat stat);

        int IComparable<IEffectStatModifierTrigger>.CompareTo(IEffectStatModifierTrigger other)
        {
            return 0;
        }
    }
}