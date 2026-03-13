using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class ChinookSalmonBeforeDead : Effect
    {
        private int newPosition;
        
        public ChinookSalmonBeforeDead(PieceLogic piece, int position) : base(-1, 1, piece, "effect_chinook_salmon_before_dead")
        {
            newPosition = position;
        }
        
        
    }
}