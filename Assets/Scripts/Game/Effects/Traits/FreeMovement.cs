using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using System.Collections.Generic;

namespace Game.Effects.Traits
{
    public class FreeMovement : Effect, IApplyEffect
    {
        private readonly List<EffectName> blockedEffects = new()
        {
            EffectName.Slow,
            EffectName.Haste
        };

        public FreeMovement(PieceLogic piece) : base(-1, 1, piece, EffectName.FreeMovement)
        { }

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