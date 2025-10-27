using UnityEngine;
using Game.Piece.PieceLogic;
using Game.Action.Internal;
using Game.Action;
using System.Collections.Generic;

namespace Game.Effects.Traits
{
    public class Sanity : Effect, IApplyEffect
    {
        private readonly List<EffectName> blockedEffects = new()
        {
            //EffectName.Frenzied,
            EffectName.Controlled,
            //EffectName.Fear,
            EffectName.Taunted
        };
        public Sanity(sbyte duration, PieceLogic owner) : base(duration, 1, owner, EffectName.Sanity)
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (blockedEffects.Contains(effect.EffectName))
            {
                applyEffect.Result = ActionResult.Failed;
            }
        }
    }
}

