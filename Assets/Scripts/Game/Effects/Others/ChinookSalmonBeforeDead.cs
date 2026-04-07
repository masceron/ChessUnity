using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;    

namespace Game.Effects.Others
{
    public class ChinookSalmonBeforeDead : Effect, IBeforePieceActionTrigger, IBeforeDestroyOrKill
    {
        private int newPosition;
        
        public ChinookSalmonBeforeDead(PieceLogic piece, int position) : base(-1, 1, piece, "effect_chinook_salmon_before_dead")
        {
            newPosition = position;
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration; // TODO: Check priority.
        
        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetAsPiece() != Piece) return;

            bool isValid = BoardUtils.VerifyIndex(newPosition)
                    && BoardUtils.IsActive(newPosition)
                    && BoardUtils.PieceOn(newPosition) == null;

            if (!isValid) return; 

            action.Result = ResultFlag.Blocked;
            ActionManager.EnqueueAction(new NormalMove(Piece, newPosition));
        }

        public void OnCallBeforeDestroyOrKill(IInternal action)
        {
            if (action is not Action.Action act) return;

            bool isValid = BoardUtils.VerifyIndex(newPosition)
                    && BoardUtils.IsActive(newPosition)
                    && BoardUtils.PieceOn(newPosition) == null;

            if (!isValid) return; 

            act.Result = ResultFlag.Blocked;
            ActionManager.EnqueueAction(new NormalMove(Piece, newPosition));
        }
        
        
    }
}