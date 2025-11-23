using MemoryPack;

namespace Game.Save.Relics
{
    [MemoryPackable]
    public partial struct Relic
    {
        public readonly string Type;

        public Relic(string type)
        {
            Type = type;
        }
    }
}