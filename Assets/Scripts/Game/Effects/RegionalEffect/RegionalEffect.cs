using Game.Managers;

namespace Game.Effects.RegionalEffect
{
    public abstract class RegionalEffect
    {
        public RegionalEffectType Type { get; set; }
        public RegionalEffect(RegionalEffectType type)
        {
            Type = type;
            MatchManager.Ins.GameState.OnIncreaseTurn += ApplyEffect;
        }
        protected abstract void ApplyEffect(int currentTurn);

        public override bool Equals(object obj)
        {
            if (obj is RegionalEffect other)
                return Type == other.Type;
            return false;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public static bool operator ==(RegionalEffect a, RegionalEffect b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Type == b.Type;
        }

        public static bool operator !=(RegionalEffect a, RegionalEffect b)
        {
            return !(a == b);
        }
    }

    public enum RegionalEffectType
    {
        Whirpool,
        PsionicShock,
        BloodMoon,
        DjinnBlessing,
        RedTide,
        None,
    }
}
