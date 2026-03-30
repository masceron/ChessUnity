using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
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
            if (action.GetMakerAsPiece() != Piece) return;

            Debug.Log(action.GetMakerAsPiece().Type + " made a quiet action, check for painted greenling passive");

            if (HasFormation(action.GetTargetPos())) 
            {
                var formation = GetFormation(action.GetTargetPos());
                if (formation.Color != action.GetMakerAsPiece().Color) return;

                var listPieces = SkillRangeHelper.GetActiveAllyPieceInRadius(action.GetTargetPos(), GetStat(EffectStat.Radius));
                foreach (var pos in listPieces)
                {
                    var p = PieceOn(pos);
                    if (p == null) continue;

                    Debug.Log("add multicast to " + p.Type);

                    ActionManager.EnqueueAction(new ApplyEffect(new Multicast(p, GetStat(EffectStat.Number), GetStat(EffectStat.Duration))));
                }
            }
        }
    }
}