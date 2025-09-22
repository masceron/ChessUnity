using Game.Relics;
using MemoryPack;

namespace Game.Save.Relics
{
    [MemoryPackable]
    public partial struct Relic
    {
        public readonly RelicType Type;

        public Relic(RelicType type)
        {
            Type = type;
        }
    }
}