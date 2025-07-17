using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;

namespace Game.Board.Effects.Buffs
{
    public class HardenedShield: Effect
    {
        public HardenedShield(sbyte duration, PieceLogic.PieceLogic piece) : base(duration, 1, piece, EffectType.HardenedShield)
        {}
        
        public override void OnCall(Action.Action action)
        {
            if (action.To != Piece.pos || action.Success != ActionResult.Succeed) return;
            action.Success = ActionResult.Failed;
            
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(2, MatchManager.GameState.MainBoard[action.Caller])));
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }
        
    }
}