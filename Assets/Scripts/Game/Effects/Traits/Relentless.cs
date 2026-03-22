using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Relentless : Effect, IBeforePieceActionTrigger, IAfterPieceActionTrigger
    {
        public Relentless(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, "effect_relentless")
        {
            SetStat(EffectStat.Stack, deathDefianceCount);
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target != Piece.Pos) return;
            if (action.Result != ResultFlag.SurvivedHit) return;

            var target = PieceOn(action.Maker);
            if (target?.Effects != null && target.Effects.All(e => e.EffectName != "effect_snapping_strike"))
                ActionManager.EnqueueAction(new KillPiece(action.Maker));

            if (GetStat(EffectStat.Stack) <= 0) ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
        }

        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Reaction;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (Piece.Effects.Any(e => e.EffectName == "effect_shield")
                || Piece.Effects.Any(e => e.EffectName == "effect_carapace")
                || Piece.Effects.Any(e => e.EffectName == "effect_hardened_shield")) return;
            if (action.Target != Piece.Pos || action.Maker == action.Target) return;
            if (GetStat(EffectStat.Stack) <= 0) return;

            action.Result = ResultFlag.SurvivedHit;
            SetStat(EffectStat.Stack, GetStat(EffectStat.Stack) - 1);
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}