using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalSpinePassive : Effect, IBeforeApplyEffectTrigger
    {
        public MetalSpinePassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_spine_passive")
        { }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_broken")
            {
                applyEffect.Result = Action.ResultFlag.Untouchable;
            }
        }
    }
}