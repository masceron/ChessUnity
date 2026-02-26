namespace Game.Relics.Commons
{
    public class RelicConfig
    {
        public readonly bool Color;
        public readonly string Type;
        public int TimeCooldown;

        public RelicConfig(string t, bool c, int timeCooldown)
        {
            Type = t;
            Color = c;
            TimeCooldown = timeCooldown;
        }
    }
}