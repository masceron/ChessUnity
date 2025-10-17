using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class SmallChargingMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var (rank, file) = RankFileOf(pos);
            var moveRange = piece.GetMoveRange(ref index);
            
            // Đi 3 ô phía trước và 2 ô phía sau
            const int forwardRange = 2;
            const int backwardRange = 1;

            switch (color)
            {
                // ⬆️ Quân màu đen (color == false) đi lên
                case false:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, forwardRange + moveRange))
                    {
                        if (!MakeMove(IndexOf(rankOff, fileOff))) break;
                    }
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, backwardRange + moveRange))
                    {
                        if (!MakeMove(IndexOf(rankOff, fileOff))) break;
                    }
                    break;

                // ⬇️ Quân màu trắng (color == true) đi xuống
                case true:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, forwardRange + moveRange))
                    {
                        if (!MakeMove(IndexOf(rankOff, fileOff))) break;
                    }
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, backwardRange + moveRange))
                    {
                        if (!MakeMove(IndexOf(rankOff, fileOff))) break;
                    }
                    break;
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
            var piece = PieceOn(pos);
            var color = piece.Color;
            var (rank, file) = RankFileOf(pos);
            var attackRange = piece.AttackRange;
            
            const int forwardRange = 3;
            const int backwardRange = 2;

            switch (color)
            {
                // ⬆️ Quân đen đi lên
                case false:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, forwardRange + attackRange))
                    {
                        if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                    }
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, backwardRange + attackRange))
                    {
                        if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                    }
                    break;

                // ⬇️ Quân trắng đi xuống
                case true:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, forwardRange + attackRange))
                    {
                        if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                    }
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, backwardRange + attackRange))
                    {
                        if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                    }
                    break;
            }

            return;

            bool MakeCapture(int index)
            {
                var p = PieceOn(index);
                if (p == null) return true;
                if (!IsActive(index)) return false;
                if (p.Color != color)
                {
                    list.Add(new NormalCapture(pos, index));
                }

                return false;
            }
        }
    }
}
