using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class ColdBloodedPassive : Effect, IBeforeApplyEffectTrigger
    {
        public ColdBloodedPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_cold_blooded_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_shortreach") applyEffect.Result = ResultFlag.Unshaken;
        }
    }
}