using Game.Movesets;

namespace Game.Piece.PieceLogic.Summon
{
    public class MedicalLeech: PieceLogic
    {
        public MedicalLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
        }
    }
}