using MemoryPack;

namespace Game.Relics.Commons
{
    [MemoryPackable]
    public partial class RelicConfig
    {
        public readonly bool Color;
        public readonly string Type;
        public readonly int TimeCooldown;

        public RelicConfig(string type, bool color, int timeCooldown)
        {
            Type = type;
            Color = color;
            TimeCooldown = timeCooldown;
        }
    }
}