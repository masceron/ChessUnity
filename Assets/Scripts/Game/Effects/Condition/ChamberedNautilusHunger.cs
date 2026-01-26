using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Condition
{
    public class ChamberedNautilusHunger : Effect, IAfterPieceActionEffect
    {
        public ChamberedNautilusHunger(PieceLogic piece, sbyte stack = 1) : base(-1, stack, piece, "effect_chambered_nautilus_hunger")
        {
            
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 25;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos && action.Result == ResultFlag.Success)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece), Piece));
            }
        }
    }
}