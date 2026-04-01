using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    public class Sanity : Effect, IBeforeApplyEffectTrigger
    {
        private static readonly List<string> BlockedEffects = new()
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

            if (BlockedEffects.Contains(effect.EffectName)) applyEffect.Result = ResultFlag.Incorruptible;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}