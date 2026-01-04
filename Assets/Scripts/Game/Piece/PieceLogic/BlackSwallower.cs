using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallower: Commons.PieceLogic
    {
        public BlackSwallower(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BlackSwallowerPassive(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new BlackSwallowerVengeful(this)));
        }

    }
}