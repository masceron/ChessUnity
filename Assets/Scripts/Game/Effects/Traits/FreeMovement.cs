using Game.Action;
using Game.Action.Internal;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class FreeMovement : Effect, IApplyEffect
    {
        private readonly List<string> blockedEffects = new()
        {
            "effect_slow",
            "effect_haste"
        };

        public FreeMovement(PieceLogic piece) : base(-1, 1, piece, "effect_free_movement")
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