using Game.Action;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Common;
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