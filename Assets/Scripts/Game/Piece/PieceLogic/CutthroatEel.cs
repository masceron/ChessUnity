using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Condition;
using Game.Effects.Debuffs;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class CutthroatEel : Commons.PieceLogic, IPieceWithSkill
    {
        public CutthroatEel(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Nocturnal(-1, 1, this, "effect_nocturnal")));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Pos), FileOf(Pos), 4))
                    {
                        var targetPos = IndexOf(rank, file);
                        if (!IsActive(targetPos)) return;
                        var pieceOn = PieceOn(targetPos);

                        if (pieceOn != null && pieceOn.Color != Color &&
                            pieceOn.Effects.Any(e => e.EffectName == "effect_bleeding"))
                            foreach (var effect in pieceOn.Effects)
                                if (effect is Bleeding)
                                {
                                    list.Add(new CutthroatEelActive(this, pieceOn));
                                    break;
                                }
                    }
                }
                else
                {
                    var (rank, file) = RankFileOf(Pos);
                    var enemiesWithBleeding = GetPiecesInRadius(rank, file, 4, p => p != null && p.Color != Color);

                    // Filter kẻ địch với Bleeding effect
                    var validTargets = new List<(Commons.PieceLogic piece, Bleeding bleeding)>();
                    foreach (var enemy in enemiesWithBleeding)
                    {
                        if (!IsActive(enemy.Pos)) return;
                        foreach (var effect in enemy.Effects)
                            if (effect is Bleeding bleeding)
                            {
                                validTargets.Add((enemy, bleeding));
                                break;
                            }
                    }

                    if (validTargets.Count == 0) return;

                    // List A: Bleeding <= 3
                    var listA = validTargets.Where(t => t.bleeding.Strength <= 3).ToList();

                    List<(Commons.PieceLogic piece, Bleeding bleeding)> candidates;

                    if (listA.Count > 0)
                    {
                        // Tìm candidate có value cao nhất
                        var maxValue = listA.Max(t => t.piece.GetValueForAI());
                        candidates = listA.Where(t => t.piece.GetValueForAI() == maxValue).ToList();
                    }
                    else
                    {
                        // List B: Bleeding >= 3
                        var listB = validTargets.Where(t => t.bleeding.Strength >= 3).ToList();
                        if (listB.Count == 0) return;

                        // Tìm candidate có value cao nhất
                        var maxValue = listB.Max(t => t.piece.GetValueForAI());
                        candidates = listB.Where(t => t.piece.GetValueForAI() == maxValue).ToList();
                    }

                    // Chọn 1 hoặc random
                    var chosen = candidates.Count == 1 ? candidates[0] : candidates[Random.Range(0, candidates.Count)];
                    list.Add(new CutthroatEelActive(this, chosen.piece));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}