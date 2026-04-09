using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

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
                                                       && applyEffect.GetTargetAsPiece() == Piece))
                applyEffect.Result = ResultFlag.EffectResistance;
        }
    }
}