using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class MetalRegulatorPassive : Effect, IBeforeApplyEffectTrigger
    {
        public MetalRegulatorPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_regulator_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_bleed") applyEffect.Result = ResultFlag.Untouchable;
        }
    }
}