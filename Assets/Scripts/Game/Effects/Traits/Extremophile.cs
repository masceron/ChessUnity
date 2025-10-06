using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    public class Extremophile: Effect, IApplyEffect
    {
        public Extremophile(PieceLogic piece) : base(-1, 1, piece, EffectName.Extremophile)
        {}

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (effect.Category == EffectCategory.Debuff || effect.Category == EffectCategory.Buff)
            {
                applyEffect.Result = ActionResult.Failed;
            }
        }
    }
}