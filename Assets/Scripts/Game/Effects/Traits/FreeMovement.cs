using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class FreeMovement : Effect, IBeforeApplyEffectTrigger
    {
        private readonly List<string> _blockedEffects = new()
        {
            "effect_slow",
            "effect_haste"
        };

        public FreeMovement(PieceLogic piece) : base(-1, 1, piece, "effect_free_movement")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (_blockedEffects.Contains(effect.EffectName)) applyEffect.Result = ResultFlag.Unshaken;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}