using Game.Action.Skills;
using Game.Movesets;
using Game.Action;

namespace Game.Piece.PieceLogic.Swarm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StoneGrab: PieceLogic
    {
        public StoneGrab(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {}
        
    }
}