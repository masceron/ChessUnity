using Game.Common;

namespace Game.Effects
{
    public interface IEffectStatModifier
    {
        public int Modify(EffectStat stat);
    }
}