using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrilledShark : Commons.PieceLogic, IPieceWithSkill
    {
        private readonly int step = 4;

        public FrilledShark(PieceConfig cfg) : base(cfg, KnightMoves.Quiets, KnightMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Sanity(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrilledSharkPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    // 8 directions: N, S, E, W, NE, NW, SE, SW
                    int[] dRank = { -1, 1, 0, 0, -1, -1, 1, 1 };
                    int[] dFile = { 0, 0, -1, 1, -1, 1, -1, 1 };

                    for (var dir = 0; dir < 8; dir++)
                    {
                        var r = rank + dRank[dir] * step;
                        var f = file + dFile[dir] * step;

                        if (!VerifyBounds(r) || !VerifyBounds(f)) continue;

                        var idx = IndexOf(r, f);
                        if (!IsActive(idx)) continue;

                        var piece = PieceOn(idx);
                        // Nếu có quân đứng đúng ở ô lướt thì không lướt được
                        if (piece != null) continue;
                        list.Add(new FrilledSharkActive(Pos, dRank[dir], dFile[dir]));
                    }
                }
                else
                {
                    var (rank, file) = RankFileOf(Pos);

                    // 8 directions: N, S, E, W, NE, NW, SE, SW
                    int[] dRank = { -1, 1, 0, 0, -1, -1, 1, 1 };
                    int[] dFile = { 0, 0, -1, 1, -1, 1, -1, 1 };

                    var bestScore = int.MinValue;
                    var bestDirs = new List<int>();

                    for (var dir = 0; dir < 8; dir++)
                    {
                        var targetRank = rank + dRank[dir] * step;
                        var targetFile = file + dFile[dir] * step;

                        if (!VerifyBounds(targetRank) || !VerifyBounds(targetFile)) continue;

                        var targetIdx = IndexOf(targetRank, targetFile);
                        if (!IsActive(targetIdx)) continue;
                        if (PieceOn(targetIdx) != null) continue;

                        var sumScore = 0;
                        var hasEnemy = false;

                        for (var step = 1; step <= 3; step++)
                        {
                            var r = rank + dRank[dir] * step;
                            var f = file + dFile[dir] * step;

                            if (!VerifyBounds(r) || !VerifyBounds(f)) break;

                            var idx = IndexOf(r, f);
                            if (!IsActive(idx)) break;

                            var pOn = PieceOn(idx);
                            if (pOn == null || pOn.Color == Color) continue;
                            if (pOn.Effects.Any(e => e.EffectName == "effect_extremophile")) continue;

                            hasEnemy = true;
                            sumScore += pOn.GetValueForAI();
                        }

                        if (!hasEnemy) continue;

                        if (sumScore > bestScore)
                        {
                            bestScore = sumScore;
                            bestDirs.Clear();
                            bestDirs.Add(dir);
                        }
                        else if (sumScore == bestScore)
                        {
                            bestDirs.Add(dir);
                        }
                    }

                    if (bestDirs.Count == 0) return;

                    var chosenDir = bestDirs.Count == 1
                        ? bestDirs[0]
                        : bestDirs[Random.Range(0, bestDirs.Count)];

                    list.Add(new FrilledSharkActive(Pos, dRank[chosenDir], dFile[chosenDir]));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}