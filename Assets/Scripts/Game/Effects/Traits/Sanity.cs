using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class Sanity : Effect, IBeforeApplyEffectTrigger
    {
        private readonly List<string> _blockedEffects = new()
        {
            "effect_frenzied",
            "effect_controlled",
            "effect_fear",
            "effect_taunted"
        };

        public Sanity(int duration, PieceLogic owner) : base(duration, 1, owner, "effect_sanity")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (_blockedEffects.Contains(effect.EffectName)) applyEffect.Result = ResultFlag.Incorruptible;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}