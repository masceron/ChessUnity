using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
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
            if (action.Caller != Piece.Pos) return;
            
            var behind = Piece.Color == Color.White ? PushWhite(action.To) : PushBlack(action.To);
            if (VerifyIndex(behind)) {
                var pieceBehind = MatchManager.Ins.GameState.PieceBoard[behind];
                if (pieceBehind != null && pieceBehind.Color != Piece.Color)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(pieceBehind)));
                    return;
                }
            }

            action.Result = ActionResult.Unblockable;
        }
    }
}