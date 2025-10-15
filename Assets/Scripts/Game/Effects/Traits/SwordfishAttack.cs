using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SwordfishAttack: Effect
    {
        public SwordfishAttack(PieceLogic piece) : base(-1, 1, piece, EffectName.SwordfishAttack)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Maker != Piece.Pos || action.Result == ActionResult.Failed) return;
            
            var behind = !Piece.Color ? PushWhite(action.Target) : PushBlack(action.Target);
            if (!VerifyIndex(behind)) return;
            
            var pieceBehind = PieceOn(behind);
            if (pieceBehind != null && pieceBehind.Color != Piece.Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, pieceBehind)));
            }
        }
    }
}