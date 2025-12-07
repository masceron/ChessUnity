using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UX.UI.Ingame;
using System.Linq;
using UnityEngine;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StingrayDash: Action, ISkills, IAIAction
    {
        public StingrayDash(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }
        
        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            var (rankFrom, fileFrom) = RankFileOf(Maker);
            var (rankTo, fileTo) = RankFileOf(Target);
            var board = PieceBoard();
            var caller = board[Maker];

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == caller.Color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p)));
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p)));
            }
            
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
        public void CompleteActionForAI()
        {
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;

            var (rank, file) = RankFileOf(Maker);

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
                        if (p.Color == makerPiece.Color) continue;
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
            Target = (ushort)chosen.finalIdx;
            BoardViewer.Ins.ExecuteAction(this);
        }
    
    }
}