using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Captures;
using Game.Effects.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Relentless: Effect, IBeforePieceActionTrigger, IAfterPieceActionTrigger
    {
        private int _deathDefianceCount;
        public Relentless(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, "effect_relentless")
        {
            _deathDefianceCount = deathDefianceCount;
        }

        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Reaction;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (Piece.Effects.Any(e => e.EffectName == "effect_shield") 
                || Piece.Effects.Any(e => e.EffectName == "effect_carapace") 
                || Piece.Effects.Any(e => e.EffectName == "effect_hardened_shield")) return;
            if (action.Target != Piece.Pos || action.Maker == action.Target) return;

            action.Result = ResultFlag.SurvivedHit;
            _deathDefianceCount--;
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target != Piece.Pos) return;
            if (action.Result != ResultFlag.SurvivedHit) return; 

            var target = PieceOn(action.Maker);
            if (target?.Effects != null && target.Effects.All(e => e.EffectName != "effect_snapping_strike"))
            {
                ActionManager.EnqueueAction(new KillPiece(action.Maker));
            }
            
            if (_deathDefianceCount <= 0)
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}