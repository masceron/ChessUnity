using Game.Triggers;

namespace Game.Effects.FieldEffect
{
    public abstract class FieldEffect : Observer
    {
        protected FieldEffect(FieldEffectType type)
        {
            //Làm lại
            Type = type;
        }

        public FieldEffectType Type { get; }
        protected abstract void ApplyEffect(int currentTurn);

        public override bool Equals(object obj)
        {
            if (obj is FieldEffect other)
                return Type == other.Type;
            return false;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public static bool operator ==(FieldEffect a, FieldEffect b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Type == b.Type;
        }

        public static bool operator !=(FieldEffect a, FieldEffect b)
        {
            return !(a == b);
        }
    }

    public enum FieldEffectType
    {
        Whirlpool,
        PsionicShock,
        BloodMoon,
        DjinnBlessing,
        RedTide,
        BenthicStorm,
        None
    }
}