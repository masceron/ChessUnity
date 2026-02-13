using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class ScalyheadSculpinPassive : Effect, IBeforeApplyEffectTrigger
    {
        public ScalyheadSculpinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_scalyhead_sculpin_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var effect = applyEffect.Effect;

            if (effect.EffectName == "effect_slow" || (effect.EffectName == "effect_stun"
                                                       && BoardUtils.PieceOn(applyEffect.Target) == Piece))
                applyEffect.Result = ResultFlag.EffectResistance;
        }
    }
}