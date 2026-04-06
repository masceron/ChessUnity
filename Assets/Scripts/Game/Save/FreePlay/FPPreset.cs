using Game.Effects.FieldEffect;
using Game.Save.Army;
using Game.Save.Relics;
using MemoryPack;

namespace Game.Save.FreePlay
{
    [MemoryPackable]
    public partial struct FPPreset
    {
        public Relic Relic, EnemyRelic;
        public string Name;
        public int BoardSize;
        public Troop[] Troops, EnemyTroops;
        public FieldEffectType FieldEffect;
    }
}