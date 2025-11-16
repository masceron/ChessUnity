using Game.Effects.RegionalEffect;
using Game.Save.Relics;
using MemoryPack;

namespace Game.Save.Army
{
    [MemoryPackable]
    public partial struct FreePlayPreset //hiện tại chưa dùng
    {
        public Relic? PlayerRelic, EnemyRelic;
        public RegionalEffectType regionalEffect;
    }
}