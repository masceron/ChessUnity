using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class PawnPushMoves
    {
        public static int Quiets(List<int> list, int pos)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var (rank, file) = RankFileOf(pos);
            var moveRange = piece.GetMoveRange();

            switch (color)
            {
                case false:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                        if (!MakeMove(IndexOf(rankOff, fileOff)))
                            break;
                    break;
                case true:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                        if (!MakeMove(IndexOf(rankOff, fileOff)))
                            break;
                    break;
            }

            return 5 * moveRange;

            bool MakeMove(int index)
            {
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p != null) return false;
                list.Add(index);
                return true;
            }
        }

        public static int Captures(List<int> list, int pos)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var (rank, file) = RankFileOf(pos);
            var moveRange = piece.GetAttackRange();

            switch (color)
            {
                case false:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                        if (!MakeCapture(IndexOf(rankOff, fileOff)))
                            break;
                    break;
                case true:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
                        if (!MakeCapture(IndexOf(rankOff, fileOff)))
                            break;
                    break;
            }

            return 5 * moveRange;

            bool MakeCapture(int index)
            {
                if (!IsActive(index)) return false;
                var p = PieceOn(index);
                if (p == null)
                {
                    list.Add(index);
                    return true;
                }
                list.Add(index);
                return false;
            }
        }
    }
}