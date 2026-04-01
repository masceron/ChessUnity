using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class BlackPrinceCopepodPassive : Effect, IStartTurnTrigger
    {
        public BlackPrinceCopepodPassive(PieceLogic piece) : base(-1, 1, piece, "effect_black_prince_copepod_passive")
        {
            SetStat(EffectStat.Radius, 4);
            SetStat(EffectStat.Range, 1, 1);
        }

        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Other;

        StartTurnEffectType IStartTurnTrigger.StartTurnEffectType => StartTurnEffectType.StartOfAllyTurn;

        void IStartTurnTrigger.OnCallStart(Action.Action lastMainAction)
        {
            var listRedCopepod = GetPiecesInRadius(RankOf(Piece.Pos), FileOf(Piece.Pos), GetStat(EffectStat.Radius), p => p != null && p.Type == "piece_red_copepod");

            foreach (var redCopepod in listRedCopepod)
            {
                redCopepod.SetAttackRange(redCopepod.GetAttackRange() + GetStat(EffectStat.Range));
                SetCooldown(Piece, ((IPieceWithSkill)Piece).TimeToCooldown - 2);
            }
        }
    }
}