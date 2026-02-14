using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class TidalRetinaPassive : Effect, IMoveRangeModifierTrigger
    {
        public TidalRetinaPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_tidal_retina_passive")
        {
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + Strength;
        }
    }
}