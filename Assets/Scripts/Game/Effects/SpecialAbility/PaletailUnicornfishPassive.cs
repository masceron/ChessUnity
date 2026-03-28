using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
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
            if (action is ICaptures && action.GetMaker() == Piece &&
                (action.Result == ResultFlag.Blocked || action.Result == ResultFlag.Miss))
                ActionManager.EnqueueAction(
                    new ApplyEffect(new Blinded(GetStat(EffectStat.Duration), 50, action.GetTarget()), Piece));
        }

        BeforeApplyEffectTriggerPriority IBeforeApplyEffectTrigger.Priority =>
            BeforeApplyEffectTriggerPriority.Prevention;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect is Blinded) applyEffect.Result = ResultFlag.Incorruptible;
        }
    }
}