using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.SpecialAbility;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MaximaClam : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 1;
        private const int Radius = 2;
        private const int Duration = -1;

        public MaximaClam(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, ShellfishMoves.Captures)
        {
            SetStat(SkillStat.Range, Range);
            SetStat(SkillStat.Radius, Radius);
            SetStat(SkillStat.Duration, Duration);
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
            // ActionManager.ExecuteImmediately(new ApplyEffect(new Crystallized(GetStat(SkillStat.Duration), this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this)));

            ActionManager.ExecuteImmediately(new ApplyEffect(new MaximaClamPassive(GetStat(SkillStat.Radius), this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;

                var (rank, file) = RankFileOf(Pos);
                var range = GetStat(SkillStat.Range);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, range))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var target = PieceOn(index);
                    if (target == null || target.Color == Color) continue;

                    var hasBoundOrLeashed = target.Effects.Any(e =>
                        e.EffectName == "effect_bound" || e.EffectName == "effect_leashed");
                    if (!hasBoundOrLeashed) continue;

                    list.Add(new MaximaClamActive(Pos, index));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
