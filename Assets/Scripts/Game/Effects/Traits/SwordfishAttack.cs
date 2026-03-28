using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SwordfishAttack : Effect, IAfterPieceActionTrigger
    {
        public SwordfishAttack(PieceLogic piece) : base(-1, 1, piece, "effect_swordfish_capture")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.GetMaker() != Piece || action.Result != ResultFlag.Success) return;

            var behind = !Piece.Color ? PushWhite(action.Target) : PushBlack(action.Target);
            if (!VerifyIndex(behind)) return;

            var pieceBehind = PieceOn(behind);
            if (pieceBehind != null && pieceBehind.Color != Piece.Color)
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, pieceBehind), Piece));
            else
                action.Flag = ActionFlag.Unblockable;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}