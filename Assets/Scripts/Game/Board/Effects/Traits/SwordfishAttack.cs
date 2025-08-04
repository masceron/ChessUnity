using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
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
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(pieceBehind)));
            }
        }
    }
}