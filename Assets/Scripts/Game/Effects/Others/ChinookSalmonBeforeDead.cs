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
        
        private bool CanEscape()
        {
            return BoardUtils.VerifyIndex(newPosition)
                && BoardUtils.IsActive(newPosition)
                && BoardUtils.PieceOn(newPosition) == null;
        }

        private void TriggerSurvival(Action.Action incomingAction)
        {
            if (!CanEscape()) return;
            
            // TODO: Tạm dùng SurvivedHit cho cơ chế "thoát chết" của Chinook.
            incomingAction.Result = ResultFlag.SurvivedHit;
            ActionManager.EnqueueAction(new NormalMove(Piece, newPosition));
            ActionManager.EnqueueAction(new RemoveEffect(this)); 
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetAsPiece() != Piece) return;
            TriggerSurvival(action);
        }

        public void OnCallBeforeDestroyOrKill(IInternal action)
        {
            if (action is not Action.Action act) return;
            if (act.GetTargetAsPiece() != Piece) return; 
            TriggerSurvival(act);
        }
        
        
    }
}