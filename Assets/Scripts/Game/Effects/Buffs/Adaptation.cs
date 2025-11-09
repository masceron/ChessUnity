using Game.Piece.PieceLogic;
using Game.Tile;


namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adaptation: Effect, IImmunity
    {
        public Adaptation(PieceLogic piece) : base(-1, 1, piece, EffectName.Adaptation)
        {}

        public bool CheckImmunity(FormationType formationType, Effect effect)
        {
            if (effect.Category == EffectCategory.Debuff)
            {
                return true;
            }
            return false;
        }

    }
}