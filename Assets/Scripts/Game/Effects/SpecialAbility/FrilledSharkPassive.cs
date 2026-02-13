using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class FrilledSharkPassive : Effect, IAfterPieceActionTrigger
    {
        public FrilledSharkPassive(PieceLogic piece) : base(-1, 1, piece, "effect_frilled_shark_passive")
        {

        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Maker != Piece.Pos) { return; }
            var pieceOn = PieceOn(action.Target);
            if (pieceOn == null) { return; }
            if (pieceOn.Effects.Any(e => e is Relentless) && IsAlive(pieceOn))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceOn), Piece));
            }
        }
    }
}