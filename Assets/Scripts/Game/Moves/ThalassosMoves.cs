using System;
using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ThalassosMoves
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var piece = PieceOn(pos);
            var color = piece.Color;
            var moveRange = piece.GetMoveRange();
            
            var push = !color ? -1 : 1;
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= moveRange; fileOff--)
            {
                if (!MakeMove(IndexOf(rank, fileOff))) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= moveRange; fileOff++)
            {
                if (!MakeMove(IndexOf(rank, fileOff))) break;
            }
            
            for (var rankOff = rank + push; VerifyBounds(rankOff) && Math.Abs(rank - rankOff) <= moveRange; rankOff += push)
            {
                if (!MakeMove(IndexOf(rankOff, file))) break;
            }
            
            for (int rankOff = rank + push, fileOff = file - 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && Math.Abs(rank - rankOff) <= moveRange && fileOff - file <= moveRange;
                 rankOff += push, fileOff--)
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }
            
            for (int rankOff = rank + push, fileOff = file + 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && Math.Abs(rank - rankOff) <= moveRange && fileOff - file <= moveRange;
                 rankOff += push, fileOff++)
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            return;
            
            bool MakeMove(int index)
            {
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p != null)
                {
                    return false;
                }
                list.Add(new NormalMove(pos, index));
                return true;
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var piece = PieceOn(pos);
            var color = piece.Color;
            var moveRange = piece.AttackRange;
            
            var push = !color ? -1 : 1;
            
            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= moveRange; fileOff--)
            {
                if (!MakeCapture(IndexOf(rank, fileOff))) break;
            }
            
            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= moveRange; fileOff++)
            {
                if (!MakeCapture(IndexOf(rank, fileOff))) break;
            }
            
            for (var rankOff = rank + push; VerifyBounds(rankOff) && Math.Abs(rank - rankOff) <= moveRange; rankOff += push)
            {
                if (!MakeCapture(IndexOf(rankOff, file))) break;
            }
            
            for (int rankOff = rank + push, fileOff = file - 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && Math.Abs(rank - rankOff) <= moveRange && fileOff - file <= moveRange;
                 rankOff += push, fileOff--)
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            for (int rankOff = rank + push, fileOff = file + 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && Math.Abs(rank - rankOff) <= moveRange && fileOff - file <= moveRange;
                 rankOff += push, fileOff++)
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            return;
            
            bool MakeCapture(int index)
            {
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p == null) return true;
                if (p.Color != color)
                {
                    list.Add(new NormalCapture(pos, index));
                }
                return false;
            }
        }
    }
}