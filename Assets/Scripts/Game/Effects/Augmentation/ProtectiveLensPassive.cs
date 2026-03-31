using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class ProtectiveLensPassive : Effect, IBeforeApplyEffectTrigger
    {
        public ProtectiveLensPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_protective_lens_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            if (applyEffect.Effect.EffectName == "effect_blinded") applyEffect.Result = ResultFlag.EffectResistance;
        }
    }
}