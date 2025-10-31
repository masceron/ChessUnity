
using Game.Movesets;

namespace Game.Piece.PieceLogic.Swarm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StoneCrab: PieceLogic
    {
        public StoneCrab(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {}
        
    }
}