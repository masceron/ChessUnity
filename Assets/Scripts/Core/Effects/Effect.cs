namespace Core.Effect
{
    public enum EffectType : byte
    {
        None,
        Shield,
        Evasion,
        HardenedShield,
        VelkarisMarked,
        Slow,
        Controlled
    }
    public abstract class Effect
    {
        public sbyte Duration;
        public byte Strength;

        protected Effect(sbyte d, byte s)
        {
            Duration = d;
            Strength = s;
        }

        public abstract void Destruct();
    }
}