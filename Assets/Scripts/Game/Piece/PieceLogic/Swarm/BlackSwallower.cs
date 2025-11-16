using Game.Action;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Action.Internal;

namespace Game.Piece.PieceLogic.Swarm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallower: PieceLogic
    {
        public BlackSwallower(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new BlackSwallowerVengeful(this)));
        }

    }
}