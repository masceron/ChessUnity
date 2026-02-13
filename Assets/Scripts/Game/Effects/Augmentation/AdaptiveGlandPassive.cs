using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Triggers;

namespace Game.Effects.Augmentation
{
    public class AdaptiveGlandPassive : Effect, IBeforeApplyEffectTrigger
    {
        public AdaptiveGlandPassive(PieceLogic piece) : base(-1, 1, piece, "effect_adaptive_gland_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(Piece)));
        }
    }
}