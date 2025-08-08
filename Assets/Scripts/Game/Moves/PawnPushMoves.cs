using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class PawnPushMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var (rank, file) = RankFileOf(pos);
            var moveRange = piece.GetMoveRange(ref index);

            switch (color)
            {
                case false:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                    {
                        if (!MakeMove(IndexOf(rankOff, fileOff))) break;
                    }
                    break;
                case true:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
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
            var moveRange = piece.AttackRange;

            switch (color)
            {
                case false:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
                    {
                        if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
                    }
                    break;
                case true:
                    foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
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