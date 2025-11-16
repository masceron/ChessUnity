using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class TidalRetinaPassive : Effect, IMoveRangeModifier
    {
        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + Strength;
        }
        public TidalRetinaPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_tidal_retina_passive")
        { }
    }
}

