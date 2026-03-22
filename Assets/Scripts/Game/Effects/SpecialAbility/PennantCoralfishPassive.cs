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
        private readonly int strength;
        private readonly int duration;
        public PennantCoralfishPassive(PieceLogic piece, int strength, int duration) : base(-1, 1, piece, "effect_pennant_coralfish_passive")
        {
            SetStat(EffectStat.Strength, strength);
            SetStat(EffectStat.Duration, duration);
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;

        void IAfterPieceActionTrigger.OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;

            var piece = PieceOn(action.Maker);
            if (piece == null || piece.Type != "piece_pennant_coralfish") return;

            foreach (var pos in SkillRangeHelper.GetActiveCellInRadius(action.Maker, 1))
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