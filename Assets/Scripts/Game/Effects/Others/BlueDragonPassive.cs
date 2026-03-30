using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class BlueDragonPassive : Effect, IAfterPieceActionTrigger
    {
        public BlueDragonPassive(PieceLogic piece) : base(-1, 1, piece, "effect_blue_dragon_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.GetMakerAsPiece() == Piece && action.Result == ResultFlag.Success)
                ActionManager.EnqueueAction(new Purify(Piece, Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 10;
        }
    }
}