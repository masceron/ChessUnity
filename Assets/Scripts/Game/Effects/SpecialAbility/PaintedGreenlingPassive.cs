using System.Collections.Generic;
using System.Net;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class PaintedGreenlingPassive : Effect, IAfterPieceActionTrigger
    {
        public PaintedGreenlingPassive(PieceLogic piece, int number, int duration, int radius) : base(-1, 1, piece, "effect_painted_greenling_passive")
        {
            SetStat(EffectStat.Number, number);
            SetStat(EffectStat.Duration, duration);
            SetStat(EffectStat.Radius, radius);
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;

        void IAfterPieceActionTrigger.OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;
            if (action.Maker != Piece.Pos) return;

            if (HasFormation(action.Target)) 
            {
                var formation = GetFormation(action.Target);
                if (formation.Color != PieceOn(action.Maker).Color) return;

                var listPieces = SkillRangeHelper.GetActiveAllyPieceInRadius(action.Target, GetStat(EffectStat.Radius));
                foreach (var pos in listPieces)
                {
                    var p = PieceOn(pos);
                    if (p == null) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Multicast(p, GetStat(EffectStat.Number), GetStat(EffectStat.Duration))));
                }
            }
        }
    }
}