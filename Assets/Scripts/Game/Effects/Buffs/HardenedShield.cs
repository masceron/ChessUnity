using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HardenedShield: Effect
    { 
        public HardenedShield(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, "effect_hardened_shield")
        {}
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed) return;
            action.Result = ActionResult.Failed;
            
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, BoardUtils.PieceOn(action.Maker))));

            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }
    }
}