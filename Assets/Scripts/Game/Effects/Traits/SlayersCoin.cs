using Game.Action;
using Game.Common;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commanders;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SlayersCoin: Effect
    {
        public SlayersCoin(PieceLogic piece) : base(-1, 1, piece, EffectName.SlayersCoin)
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action.Result == ActionResult.Failed) return;
            
            var caller = BoardUtils.PieceOn(action.Maker);
            var captured = BoardUtils.PieceOn(action.Target);
            
            if (caller.Color == Piece.Color && caller.PieceRank < captured.PieceRank) ((Chrysos)Piece).Coin += 1;
        }
    }
}