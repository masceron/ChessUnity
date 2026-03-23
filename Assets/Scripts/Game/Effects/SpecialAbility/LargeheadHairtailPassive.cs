using Game.Action.Skills;
using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class LargeheadHairtailPassive : Effect, IStartTurnTrigger, IAttackRangeModifier
    {
        private const int Radius = 4;
        private int bonus = 0;
        
        public LargeheadHairtailPassive(PieceLogic piece) : base(-1, 1, piece, "effect_largehead_hairtail_passive")
        {
            SetStat(EffectStat.Radius, Radius);
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType { get; }
        public void OnCallStart(Action.Action lastMainAction)
        {
            bonus = 0; // không cộng dồn
            var (rank, file) = RankFileOf(Piece.Pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, GetStat(EffectStat.Radius)))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn != null && pOn.Color != Piece.Color && pOn.Effects.Any(e => e.EffectName != "effect_blinded"))
                {
                    bonus++;
                }
            }
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + bonus;
        }
        
    }
}