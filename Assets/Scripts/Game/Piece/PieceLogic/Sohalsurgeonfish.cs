using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SohalSurgeonfish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 6;
        private const int Duration = 5;
        public SohalSurgeonfish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            SetStat(SkillStat.Range, Range);
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, GetStat(SkillStat.Range));
                    foreach (var target in targets)
                    {
                        if (target.Effects.Any(e => e.EffectName == "effect_slow"))
                            list.Add(new SohalSurgeonfishActive(this, target, Duration));
                    }
                }
                else
                {
                    if (!excludeEmptyTile)
                    {
                        var (rank, file) = RankFileOf(Pos);
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, GetStat(SkillStat.Range)))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            list.Add(new SohalSurgeonfishActive(this, PieceOn(index), Duration));
                        }
                    }
                    else
                    {
                        var listPieces = new List<Commons.PieceLogic>();

                        foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), GetStat(SkillStat.Range)))
                        {
                            var idx = IndexOf(rank, file);
                            var pOn = PieceOn(idx);
                            if (pOn != null && pOn.Color != Color)
                                if (pOn.Effects != null && !pOn.Effects.Any(e => e.EffectName == "effect_leashed")
                                                        && pOn.Effects.Any(e => e.EffectName == "effect_slow"))
                                    listPieces.Add(pOn);
                        }

                        if (listPieces.Count == 0) return;
                        var maxValue = listPieces.Max(p => p.GetValueForAI());
                        var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                        if (bestPieces.Count == 0) return;
                        var random = new Random();
                        var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
                        list.Add(new SohalSurgeonfishActive(this, selectedPiece, Duration));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}