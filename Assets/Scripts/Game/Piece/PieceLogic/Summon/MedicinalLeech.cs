using Game.Movesets;

namespace Game.Piece.PieceLogic.Summon
{
    public class MedicinalLeech: PieceLogic
    {
        public MedicinalLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, HorseLeechMoves.Captures)
        {
        }
    }
}