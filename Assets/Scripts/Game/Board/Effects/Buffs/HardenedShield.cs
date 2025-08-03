using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.Piece.PieceLogic;
using Game.Common;

namespace Game.Board.Effects.Buffs
{
    public class HardenedShield: Effect
    { 
        public HardenedShield(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, EffectName.HardenedShield)
        {}
        
        public override void OnCall(Action.Action action)
        {
            if (action == null || action.To != Piece.Pos || action.Result != ActionResult.Succeed) return;
            action.Result = ActionResult.Failed;
            
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, BoardUtils.PieceOn(action.From))));

            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }
    }
}