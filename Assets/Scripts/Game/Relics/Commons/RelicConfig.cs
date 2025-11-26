
namespace Game.Relics.Commons
{
    public class RelicConfig
    {
        public readonly string Type;
        public readonly bool Color;
        public sbyte TimeCooldown;

        public RelicConfig(string t, bool c, sbyte timeCooldown)
        {
            Type = t;
            Color = c;
            TimeCooldown = timeCooldown;
        }
    }
}
