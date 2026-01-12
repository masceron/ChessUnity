using Game.Action.Internal;
using System.Collections.Generic;
using Game.Action;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class Sanity : Effect, IApplyEffect
    {
        private readonly List<string> blockedEffects = new()
        {
            "effect_frenzied",
            "effect_controlled",
            "effect_fear",
            "effect_taunted"
        };
        public Sanity(sbyte duration, PieceLogic owner) : base(duration, 1, owner, "effect_sanity")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (blockedEffects.Contains(effect.EffectName))
            {
                applyEffect.Result = ResultFlag.Incorruptible;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}

