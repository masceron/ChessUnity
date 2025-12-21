using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class HemolymphFilterPassive : Effect
    {
        public HemolymphFilterPassive(PieceLogic piece) : base(-1, 1, piece, "effect_hemolymph_filter_passive")
        {
        }
    }
}