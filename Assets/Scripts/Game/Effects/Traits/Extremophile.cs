using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using System.Collections.Generic;

namespace Game.Effects.Traits
{
    public class Extremophile: Effect, IApplyEffect
    {
        private readonly List<EffectCategory> blockCategories = new()
        {
            EffectCategory.Buff,
            EffectCategory.Debuff
        };

        public Extremophile(PieceLogic piece) : base(-1, 1, piece, EffectName.Extremophile)
        {}

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var effect = applyEffect.Effect;
            if (Piece != effect.Piece) return;
            

            if (blockCategories.Contains(effect.Category))
            {
                applyEffect.Result = ActionResult.Failed;
            }
        }
    }
}