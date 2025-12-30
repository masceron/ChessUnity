using System.Linq;
using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;
using Game.Effects.Buffs;
using Game.Action.Internal;
using Game.Action;
using Game.Effects.Debuffs;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Stingray : Commons.PieceLogic, IPieceWithSkill
    {
        public Stingray(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Slow(-1, 3, this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                var (rank, file) = RankFileOf(Pos);
                if (isPlayer)
                {

                    var board = PieceBoard();
                    var active = ActiveBoard();

                    for (var rankTo = rank - 2; rankTo <= rank + 2; rankTo += 2)
                    {
                        if (!VerifyBounds(rankTo)) continue;
                        for (var fileTo = file - 2; fileTo <= file + 2; fileTo += 2)
                        {
                            if (!VerifyBounds(fileTo)) continue;
                            if (rankTo == rank && fileTo == file) continue;
                            var posTo = IndexOf(rankTo, fileTo);

                            if (board[posTo] == null && active[posTo])
                            {
                                list.Add(new StingrayDash(Pos, posTo));
                            }
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        var board = PieceBoard();
                        var active = ActiveBoard();

                        for (var rankTo = rank - 2; rankTo <= rank + 2; rankTo += 2)
                        {
                            if (!VerifyBounds(rankTo)) continue;
                            for (var fileTo = file - 2; fileTo <= file + 2; fileTo += 2)
                            {
                                if (!VerifyBounds(fileTo)) continue;
                                if (rankTo == rank && fileTo == file) continue;
                                var posTo = IndexOf(rankTo, fileTo);
                                if (active[posTo])
                                {
                                    list.Add(new StingrayDash(Pos, posTo));
                                }
                            }
                        }
                    }
                    // candidates: tuple (finalIndex, bestEnemyValue)
                    var candidates = new System.Collections.Generic.List<(int finalIdx, int bestValue)>();

                    for (int dr = -2; dr <= 2; dr += 2)
                    {
                        for (int df = -2; df <= 2; df += 2)
                        {
                            if (dr == 0 && df == 0) continue;
                            int rankTo = rank + dr;
                            int fileTo = file + df;
                            if (!VerifyBounds(rankTo) || !VerifyBounds(fileTo)) continue;
                            int finalIdx = IndexOf(rankTo, fileTo);
                            if (!IsActive(finalIdx)) continue;
                            if (PieceOn(finalIdx) != null) continue; // destination must be empty

                            // traverse path stepwise (one-step increments) and find enemies
                            int stepRank = dr == 0 ? 0 : (dr > 0 ? 1 : -1);
                            int stepFile = df == 0 ? 0 : (df > 0 ? 1 : -1);
                            int curR = rank;
                            int curF = file;
                            int bestEnemyValue = int.MinValue;
                            bool foundEnemy = false;

                            while (curR != rankTo || curF != fileTo)
                            {
                                curR += stepRank;
                                curF += stepFile;
                                if (!VerifyBounds(curR) || !VerifyBounds(curF)) break;
                                int idx = IndexOf(curR, curF);
                                var p = PieceOn(idx);
                                if (p == null) continue;
                                if (p.Color == Color) continue;
                                // enemy found on path
                                foundEnemy = true;
                                int val = p.GetValueForAI();
                                if (val > bestEnemyValue) bestEnemyValue = val;
                            }

                            if (foundEnemy)
                            {
                                candidates.Add((finalIdx, bestEnemyValue));
                            }
                        }
                    }

                    if (candidates.Count == 0) { Debug.LogError("[Stingray] No candidate!"); }

                    // choose candidate with max bestValue, break ties randomly
                    int maxVal = candidates.Max(c => c.bestValue);
                    var top = candidates.Where(c => c.bestValue == maxVal).ToList();
                    var chosen = top.Count == 1 ? top[0] : top[Random.Range(0, top.Count)];
                    list.Add(new StingrayDash(Pos, chosen.finalIdx));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}