using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class ScalyheadSculpinPassive : Effect, IApplyEffect
    {
        public ScalyheadSculpinPassive(PieceLogic piece) : base (-1, 1, piece, "effect_scalyhead_sculpin_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            Effect effect = applyEffect.Effect;

            if (effect.EffectName == "effect_slow" || effect.EffectName == "effect_stun"
                && BoardUtils.PieceOn(applyEffect.Target) == Piece)
            {
                applyEffect.Result = Action.ResultFlag.EffectResistance;
            }
        }
    }
}