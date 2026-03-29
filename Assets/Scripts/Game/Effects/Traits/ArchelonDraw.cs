using Game.Action;
using Game.Action.Captures;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArchelonDraw : Effect, IBeforePieceActionTrigger
    {
        public ArchelonDraw(PieceLogic piece) : base(-1, 1, piece, "effect_archelon_draw")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Redirection;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetingType() != TargetingType.Unit ||
                Distance(action.GetTargetPos(), Piece.Pos) > 2 ||
                action.GetTarget().Color != Piece.Color ||
                action.GetTarget() == Piece)
                return;

            action.ChangeTarget(Piece);
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 100;
        }
    }
}