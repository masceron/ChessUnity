using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class BishopMoves
    {
        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var piece = PieceOn(pos);
            var rank = RankOf(pos);
            var file = FileOf(pos);
            var maxRange = piece.GetMoveRange(ref index);
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, maxRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, maxRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, maxRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, maxRange))
            {
                if (!MakeMove(IndexOf(rankOff, fileOff))) break;
            }

            return 10 + 5 * maxRange;
            
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
            var moveRange = piece.AttackRange;

            var (rank, file) = RankFileOf(pos);
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpLeft(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownLeft(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.UpRight(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.DownRight(rank, file, moveRange))
            {
                if (!MakeCapture(IndexOf(rankOff, fileOff))) break;
            }

            return 10 + 5 * moveRange;

            bool MakeCapture(int index)
            {
                var p = PieceOn(index);
                if (p == null) 
                {
                    if (!isPlayer) 
                    {
                        list.Add(new NormalCapture(pos, index));
                        return false;
                    } else
                    {
                        return true;
                    }
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