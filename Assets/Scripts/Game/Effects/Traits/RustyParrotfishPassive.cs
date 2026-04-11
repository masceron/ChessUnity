using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.SpecialAbility
{
    public class RustyParrotfishPassive : Effect
    {
        public RustyParrotfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_rusty_parrotfish_passive")
        {
            // Không làm gì cả vì if-else đã đặt ở Demolisher
        }
    }
}