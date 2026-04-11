using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Demolisher : Effect, IAfterPieceActionTrigger, IOnMoveGenTrigger
    {
        public Demolisher(PieceLogic piece) : base(-1, 1, piece, "effect_demolisher")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            var maker = action.GetMakerAsPiece();
            if (action is DestroyConstruct && maker == Piece && action.Result == ResultFlag.Success)
            {
                if ((Piece is RedtailParrotfish || Piece is RustyParrotfish) && action.GetTargetAsPiece() is StoneWall) return;
                // Giết chính mình
                ActionManager.EnqueueAction(new KillPiece(null, Piece));
            }
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
                if (actions[i] is ICaptures)
                    actions[i] = new DestroyConstruct(Piece, actions[i].GetTargetAsPiece());
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}