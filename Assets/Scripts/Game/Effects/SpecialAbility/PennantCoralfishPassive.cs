using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class PennantCoralfishPassive : Effect, IAfterPieceActionTrigger
    {
        public PennantCoralfishPassive(PieceLogic piece, int strength, int duration, int radius) : base(-1, 1, piece, "effect_pennant_coralfish_passive")
        {
            SetStat(EffectStat.Strength, strength);
            SetStat(EffectStat.Duration, duration);
            SetStat(EffectStat.Radius, radius);
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;

        void IAfterPieceActionTrigger.OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;
            if (action.Maker != Piece.Pos) return;

            var piece = PieceOn(action.Maker);
            if (piece == null || piece.Type != "piece_pennant_coralfish") return;
            //Debug.Log(piece.Type + " made a quiet action, check for pennant coralfish passive");

            //var longReach = piece.Effects.Find(e => e is LongReach) as LongReach;
            //if (longReach != null)
            //{
            //    Debug.Log("Longreach Strength: " + longReach.Strength);
            //}

            foreach (var pos in SkillRangeHelper.GetActiveCellInRadius(action.Maker, GetStat(EffectStat.Radius)))
            {
                var p = PieceOn(pos);
                if (pos == action.Maker) continue;
                if (p == null || p.Type != "piece_pennant_coralfish") continue;

                // nếu đi cạnh quân đấy nhiều lần thì có stack lên không hay chỉ được 1 lần ?
                ActionManager.EnqueueAction(new ApplyEffect(new LongReach(piece, GetStat(EffectStat.Duration), GetStat(EffectStat.Strength))));
                ActionManager.EnqueueAction(new ApplyEffect(new LongReach(p, GetStat(EffectStat.Duration), GetStat(EffectStat.Strength))));

            }
        }
    }
}