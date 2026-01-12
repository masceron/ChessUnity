using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HardenedShield : Effect, IBeforePieceActionEffect, IAfterPieceActionEffect
    {
        public HardenedShield(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, "effect_hardened_shield")
        {
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Target != Piece.Pos || action.Result != ResultFlag.Success ||
                (action.Flag & ActionFlag.Unblockable) != 0) return;
            action.Result = ResultFlag.HardenedBlock;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures 
                || action.Target != Piece.Pos 
                || action.Result != ResultFlag.Blocked 
                || action.Result != ResultFlag.HardenedBlock
                || action.Result != ResultFlag.Evade) return;
            
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, BoardUtils.PieceOn(action.Maker))));
            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 50;
        }
    }
}