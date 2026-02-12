
namespace Game.Relics.Commons
{
    public class RelicConfig
    {
        public readonly string Type;
        public readonly bool Color;
        public int TimeCooldown;

        public RelicConfig(string t, bool c, int timeCooldown)
        {
            Type = t;
            Color = c;
            TimeCooldown = timeCooldown;
        }
    }
}
