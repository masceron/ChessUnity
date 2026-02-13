using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalMindPassive : Effect, IBeforeApplyEffectTrigger
    {
        public MetalMindPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_mind_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_frenzied") applyEffect.Result = ResultFlag.Incorruptible;
        }
    }
}