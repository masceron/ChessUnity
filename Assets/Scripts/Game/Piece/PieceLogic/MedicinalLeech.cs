using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class MedicinalLeech: Commons.PieceLogic
    {
        public MedicinalLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, HorseLeechMoves.Captures)
        {
        }
    }
}