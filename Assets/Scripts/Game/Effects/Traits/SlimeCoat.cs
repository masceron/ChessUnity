using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    public class SlimeCoat : Effect, IBeforeApplyEffectTrigger
    {
        private const int NewStack = 10;
        public SlimeCoat(PieceLogic piece) : base(-1, 1, piece, "effect_slime_coat")
        {
            
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Reaction;
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece || applyEffect.Effect is not Poison poison) return;
            poison.Stack = NewStack;
        }
    }
}