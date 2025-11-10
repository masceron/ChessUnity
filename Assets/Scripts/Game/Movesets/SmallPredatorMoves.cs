using System;
using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;
namespace Game.Movesets
{
    public class SmallPredatorMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var (rank, file) = RankFileOf(pos);
            var moveRange = piece.GetMoveRange(ref index);

            var push = !color ? -1 : 1;

            // --- Di chuyển ngang ---
            for (var i = file - moveRange + 1; i <= file + moveRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);

                if (!MakeMove(posTo)) continue;
            }

            // --- Di chuyển chéo theo hướng "push" ---
            for (var rankOff = push; Math.Abs(rankOff) <= moveRange; rankOff += push)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyBounds(rankAfter)) break;

                for (var j = file - moveRange + Math.Abs(rankOff); j <= file + moveRange - Math.Abs(rankOff); j++)
                {
                    if (!VerifyBounds(j)) continue;

                    var posTo = IndexOf(rankAfter, j);

                    if (!MakeMove(posTo)) continue;
                }
            }

            return;

            bool MakeMove(int indexTo)
            {
                if (!IsActive(indexTo)) return false;

                var p = PieceOn(indexTo);

                // Nếu có quân chắn hoặc bị chặn trên đường đi, dừng.
                if (p != null) return false;

                if (Pathfinder.LineBlocker(rank, file, RankOf(indexTo), FileOf(indexTo)).Item1 != -1)
                    return false;

                list.Add(new NormalMove(pos, indexTo));
                return true;
            }
        }
        
        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var color = piece.Color;
            var attackRange = piece.AttackRange;
            
            var push = !color ? -1 : 1;
            
            for (var i = file - attackRange + 1; i <= file + attackRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);

                var p = PieceOn(posTo);

                if (p == null || p.Color == color) continue;
                
                if (Pathfinder.LineBlocker(rank, file,
                        rank, i).Item1 != -1) continue;
                
                list.Add(new NormalCapture(pos, posTo));
            }

            for (var rankOff = push; Math.Abs(rankOff) <= attackRange; rankOff += push)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyBounds(rankAfter)) break;
                
                for (var j = file - attackRange + Math.Abs(rankOff); j <= file + attackRange - Math.Abs(rankOff); j++)
                {
                    if (!VerifyBounds(j)) continue;
                    
                    var posTo = IndexOf(rankAfter, j);
                    var p = PieceOn(posTo);
                    
                    if (p == null || p.Color == color) continue;
                    
                    if (Pathfinder.LineBlocker(rank, file,
                            rank, j).Item1 != -1) continue;
                
                    list.Add(new NormalCapture(pos, posTo));
                }
            }
        }
    }
}