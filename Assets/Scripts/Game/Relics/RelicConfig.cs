
namespace Game.Relics
{
    public class RelicConfig
    {
        public readonly RelicType Type;
        public readonly bool Color;
        public sbyte TimeCooldown;

        public RelicConfig(RelicType t, bool c, sbyte timeCooldown)
        {
            Type = t;
            Color = c;
            TimeCooldown = timeCooldown;
        }
    }
}
