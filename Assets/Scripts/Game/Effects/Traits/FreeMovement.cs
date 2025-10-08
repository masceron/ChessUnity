using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class FreeMovement : Effect, IApplyEffect
    {
        public FreeMovement(PieceLogic piece) : base(-1, 1, piece, EffectName.FreeMovement)
        { }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (effect.EffectName is EffectName.Slow or EffectName.Haste)
            {
                applyEffect.Result = ActionResult.Failed;
            }
        }
    }
}