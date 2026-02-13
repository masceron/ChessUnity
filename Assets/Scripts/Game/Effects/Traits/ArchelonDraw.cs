using Game.Action.Captures;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
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
            if (action is not ICaptures || Distance(action.Target, Piece.Pos) > 2 ||
                ColorOfPiece(action.Target) != Piece.Color ||
                action.Target == Piece.Pos)
                return;

            action.Target = Piece.Pos;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 100;
        }
    }
}