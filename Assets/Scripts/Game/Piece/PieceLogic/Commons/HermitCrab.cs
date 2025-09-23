using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Movesets;
using Game.Action;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermitCrab: PieceLogic
    {
        public HermitCrab(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {}
    }
}
