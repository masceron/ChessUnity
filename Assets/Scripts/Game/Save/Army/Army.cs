using MemoryPack;

namespace Game.Save.Army
{
    [MemoryPackable]
    public partial struct Army
    {
        public ushort BoardSize;
        public Troop[] Troops;
    }
}