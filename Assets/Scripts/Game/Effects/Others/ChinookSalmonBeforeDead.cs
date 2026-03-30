using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
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
        
        public void OnCallBeforeDestroyOrKill(IInternal action)
        {
            if (action is Action.Action act)
            {
                act.Result = ResultFlag.Blocked; // TODO: Check ResultFlag.
                ActionManager.EnqueueAction(new NormalMove(Piece.Pos, newPosition));
            }
        }
        
        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTargetAsPiece() != Piece) return;
            action.Result = ResultFlag.Blocked; // TODO: Check ResultFlag.
            ActionManager.EnqueueAction(new NormalMove(Piece.Pos, newPosition));
        }
        
        
    }
}