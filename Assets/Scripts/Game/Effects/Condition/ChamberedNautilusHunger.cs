using System;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Condition
{
    public class ChamberedNautilusHunger : Effect, IAfterPieceActionTrigger
    {
        public ChamberedNautilusHunger(PieceLogic piece, int stack = 1) : base(-1, stack, piece,
            "effect_chambered_nautilus_hunger")
        {
        }

        public AfterActionPriority Priority => throw new NotImplementedException();

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.GetMaker() as PieceLogic == Piece && action.Result == ResultFlag.Success)
                ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 25;
        }
    }
}