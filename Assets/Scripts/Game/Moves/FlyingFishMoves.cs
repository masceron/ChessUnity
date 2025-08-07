using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class FlyingFishMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            
            var board = PieceBoard();
            var active = ActiveBoard();
            var effectiveMoveRange = PieceOn(pos).EffectiveMoveRange;

            for (var rankTo = rank - effectiveMoveRange; rankTo <= rank + effectiveMoveRange; rankTo += effectiveMoveRange)
            {
                if (!VerifyBounds(rankTo)) continue;
                for (var fileTo = file - effectiveMoveRange; fileTo <= file + effectiveMoveRange; fileTo += effectiveMoveRange)
                {
                    if (!VerifyBounds(fileTo)) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    var posTo = IndexOf(rankTo, fileTo);

                    if (board[posTo] == null && active[posTo])
                    {
                        list.Add(new NormalMove(pos, posTo));
                    }
                }
            }
        }
        
        public static void Captures(List<Action.Action> list, int Pos)
        {
            var board = PieceBoard();
            var p = PieceOn(Pos);
            var color = p.Color;
            var attackRange = p.AttackRange;
            
            var ver1 = PushWhite(Pos) * attackRange;
            var ver2 = PushBlack(Pos) * attackRange;

            if (VerifyUpperIndex(ver1) && board[ver1] != null && board[ver1].Color != color)
            {
                list.Add(new NormalCapture(Pos, ver1));
            }
            
            if (ver2 > 0 && board[ver2] != null && board[ver2].Color != color)
            {
                list.Add(new NormalCapture(Pos, ver2));
            }

            var (rank, file) = RankFileOf(Pos);
                
            var fileOff1 = file - attackRange;
            var fileOff2 = file + attackRange;

            if (fileOff1 > 0)
            {
                var hoz1 = IndexOf(rank, fileOff1);
                
                if (board[hoz1] != null && board[hoz1].Color != color)
                {
                    list.Add(new NormalCapture(Pos, hoz1));
                }
            }

            if (!VerifyUpperBound(fileOff2)) return;
            
            var hoz2 = IndexOf(rank, fileOff2);
            if (board[hoz2] != null && board[hoz2].Color != color)
            {
                list.Add(new NormalCapture(Pos, hoz2));
            }
        }
    }
}