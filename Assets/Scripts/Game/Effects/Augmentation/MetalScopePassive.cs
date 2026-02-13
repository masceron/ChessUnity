using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalScopePassive : Effect, IBeforeApplyEffectTrigger
    {
        public MetalScopePassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_scope_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_marked") applyEffect.Result = ResultFlag.EffectResistance;
        }
    }
}