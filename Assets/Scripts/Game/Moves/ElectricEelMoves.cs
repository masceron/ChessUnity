using System;
using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ElectricEelMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var effectiveMoveRange = piece.EffectiveMoveRange;
            
            for (var i = file - effectiveMoveRange + 1; i <= file + effectiveMoveRange - 1; i++)
            {
                if (i == file || !VerifyBounds(i)) continue;
                var posTo = IndexOf(rank, i);
                
                if (PieceOn(posTo) != null ||
                    !IsActive(posTo)) continue;

                if (Pathfinder.LineBlocker(rank, file,
                        rank, i).Item1 != -1) continue;
                
                list.Add(new NormalMove(pos, posTo));
            }
            
            for (var i = 1; i <= effectiveMoveRange; i++)
            {
                var rankOffFront = rank + i;
                var rankOffBack = rank - i;

                for (var j = -i; j <= i; j++)
                {
                    var fileOff = file + j;
                    if (!VerifyBounds(fileOff)) continue;
                    if (VerifyBounds(rankOffFront))
                    {
                        var posOffFront = IndexOf(rankOffFront, fileOff);

                        if (Pathfinder.LineBlocker(rank, file,
                                rankOffFront, fileOff).Item1 == -1)
                        {
                            if (PieceOn(posOffFront) == null &&
                                IsActive(posOffFront))
                            {
                                list.Add(new NormalMove(pos, posOffFront));
                            }
                        }
                    }

                    if (!VerifyBounds(rankOffBack)) continue;

                    var posOffBack = IndexOf(rankOffBack, fileOff);

                    if (Pathfinder.LineBlocker(rank, file,
                            rankOffBack, fileOff).Item1 != -1) continue;
                    
                    if (PieceOn(posOffBack) == null &&
                        IsActive(posOffBack))
                    {
                        list.Add(new NormalMove(pos, posOffBack));
                    }
                }
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