using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class PaletailUnicornfishPassive : Effect, IAfterPieceActionTrigger, IBeforeApplyEffectTrigger
    {
        public PaletailUnicornfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_paletail_unicornfish_passive")
        {
            SetStat(EffectStat.Duration, 3);
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos &&
                (action.Result == ResultFlag.Blocked || action.Result == ResultFlag.Miss))
                ActionManager.EnqueueAction(
                    new ApplyEffect(new Blinded(GetStat(EffectStat.Duration), 50, PieceOn(action.Target)), Piece));
        }

        BeforeApplyEffectTriggerPriority IBeforeApplyEffectTrigger.Priority =>
            BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect is Blinded) applyEffect.Result = ResultFlag.Incorruptible;
        }
    }
}