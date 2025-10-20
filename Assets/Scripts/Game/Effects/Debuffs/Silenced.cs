using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    public class Silenced : Effect
    {
        public Silenced(PieceLogic piece) : base(-1, 1, piece, EffectName.Silenced)
        {
        }
    }
}