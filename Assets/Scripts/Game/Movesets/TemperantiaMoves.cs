using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class TemperantiaMoves
    {
        public static int Quiets(List<Action.Action> list, int pos, bool isPlayer)
        {
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange();

            var (rank, file) = RankFileOf(pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            return 15 + 5 * moveRange;

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

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var moveRange = piece.GetAttackRange();

            var (rank, file) = RankFileOf(pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            return 15 + 5 * moveRange;

            bool MakeCapture(int index)
            {
                var p = PieceOn(index);
                if (p == null) 
                {
                    if (!isPlayer)
                    {
                        list.Add(new NormalCapture(pos, index));
                        return false;
                    }
                    return true;
                }
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