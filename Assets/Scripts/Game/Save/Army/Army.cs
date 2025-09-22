using Game.Save.Relics;
using MemoryPack;

namespace Game.Save.Army
{
    [MemoryPackable]
    public partial struct Army
    {
        public string Name;
        public ushort BoardSize;
        public Troop[] Troops;
        public Relic? Relic;
    }
}