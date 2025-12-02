using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SmallChargingMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos, ref int index)
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
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateSmallChargingMovePattern(makerPos);
        }

        private List<int> GenerateSmallChargingMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);
            // Đi 3 ô phía trước và 2 ô phía sau
            positions.Add(IndexOf(rank + push * 3, file));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank - push * 2, file));
            positions.Add(IndexOf(rank - push * 1, file));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new SmallChargingMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false);

            return 10 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new SmallChargingMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true);
            return 10 + 5 * attackRange;
        }
    }
}
