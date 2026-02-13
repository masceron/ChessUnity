using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class LeviathanScalePassive : Effect, IBeforeApplyEffectTrigger
    {
        public LeviathanScalePassive(PieceLogic piece) : base(-1, 1, piece, "effect_leviathan_scale_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Reaction;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;
            var effect = applyEffect.Effect;
            if (effect is not Shield) return;
            applyEffect.Result = ResultFlag.CantApplyEffect;

            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(effect.Strength, Piece)));
        }
    }
}