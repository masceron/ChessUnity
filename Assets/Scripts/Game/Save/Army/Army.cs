using Game.Save.Relics;
using MemoryPack;

namespace Game.Save.Army
{
    [MemoryPackable]
    public partial struct Army
    {
        public string Name;

        public int BoardSize;

        //Troops là array liền kề
        public Troop[] Troops;
        public Relic? Relic;
    }
}