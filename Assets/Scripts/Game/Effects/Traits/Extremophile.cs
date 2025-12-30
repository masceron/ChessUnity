using Game.Action.Internal;
using System.Collections.Generic;
using Game.Action;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class Extremophile: Effect, IApplyEffect
    {
        private readonly List<EffectCategory> blockCategories = new()
        {
            EffectCategory.Buff,
            EffectCategory.Debuff
        };

        public Extremophile(PieceLogic piece) : base(-1, 1, piece, "effect_extremophile")
        {}

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var effect = applyEffect.Effect;
            if (Piece != effect.Piece) return;
            

            if (blockCategories.Contains(effect.Category))
            {
                applyEffect.Result = ResultFlag.Blocked;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}