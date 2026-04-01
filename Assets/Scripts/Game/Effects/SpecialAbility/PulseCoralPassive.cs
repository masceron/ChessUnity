using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class PulseCoralPassive : Effect, IStartTurnTrigger
    {
        private int count = 0;

        public PulseCoralPassive(PieceLogic piece) : base(-1, 1, piece, "effect_pulse_coral_passive")
        {
            SetStat(EffectStat.Duration, 3, 1);
            SetStat(EffectStat.Duration, 3, 2);
            SetStat(EffectStat.Radius, 2);
        }

        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAnyTurn;

        public void OnCallStart(Action.Action lastMainAction)
        {
            count++;

            if (count < 3) return;

            count = 0;

            var allies = new List<PieceLogic>();

            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Piece.Pos), FileOf(Piece.Pos), GetStat(EffectStat.Radius)))
            {
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color == Piece.Color && pieceOn != Piece)
                {
                    allies.Add(pieceOn);
                }
            }

            foreach (var ally in allies)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Rally(GetStat(EffectStat.Duration, 1), ally)));
                ActionManager.EnqueueAction(new ApplyEffect(new Momentum(GetStat(EffectStat.Duration, 2), ally)));
            }
        }
    }
}
