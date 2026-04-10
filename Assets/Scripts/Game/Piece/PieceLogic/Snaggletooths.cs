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
    public class Snaggletooths : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 2;
        public Snaggletooths(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var targets = SkillRangeHelper.GetActiveCellInRadius(Pos, Range);
                    foreach (var target in targets)
                    {
                        var piece = PieceOn(target);
                        if (piece == null) continue;
                        if (piece.Effects.Any(e => e.EffectName == "effect_bleeding"))
                            list.Add(new SnaggletoothsActive(this, piece));
                    }
                    // var (rank, file) = RankFileOf(Pos);
                    // foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                    // {
                    //     var index = IndexOf(rankOff, fileOff);
                    //     var piece = PieceOn(index);
                    //     if (piece == null) continue;
                    //     if (piece.Effects.Any(e => e.EffectName == "effect_bleeding"))
                    //     {
                    //         list.Add(new SnaggletoothsActive(Pos, index));
                    //     }
                    // }
                }
                else
                {
                    var listPieces = new List<Commons.PieceLogic>();
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        var (rank, file) = RankFileOf(Pos);
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                            list.Add(new SnaggletoothsActive(this, PieceOn(IndexOf(rankOff, fileOff))));
                    }

                    var maxValue = listPieces.Max(p => p.GetValueForAI());
                    var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                    if (bestPieces.Count == 0) return;

                    var random = new Random();
                    var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
                    list.Add(new SnaggletoothsActive(this, selectedPiece));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}