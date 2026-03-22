using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.SpecialAbility
{
    public class TurbanSnailPassive : Effect, IBeforeApplyEffectTrigger
    {
        public TurbanSnailPassive(PieceLogic piece) : base(-1, 1, piece, "effect_turban_snail_passive")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect is Infected)
            {
                applyEffect.Result = ResultFlag.CantApplyEffect;
            }
        }
    }
}