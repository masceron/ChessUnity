using Game.Action.Internal;
using System.Collections.Generic;
using Game.Action;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class Extremophile: Effect, IApplyEffect
    {
        private static readonly List<EffectCategory> BlockCategories = new()
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
            

            if (BlockCategories.Contains(effect.Category))
            {
                applyEffect.Result = ResultFlag.Untouchable;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}