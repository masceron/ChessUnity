using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using static Game.Common.BoardUtils;
using Game.Common;


namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    /// ******x*****
    /// ****xxxxx****
    /// ****xxOxx****
    /// *****xxx*****
    public static class VersatileDefenderMove
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {   
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var color = caller.Color;
            
            var MoveRange = caller.GetMoveRange(ref index);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, MoveRange))
            {
                MakeMove(rankOff, fileOff);
            }
            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank - 1, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank - 1, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }  
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank + 1, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank + 1, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }  
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, MoveRange + 1))
                {
                    MakeMove(rankOff, fileOff);
                }
            }
            return;
            
            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                
                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalMove(pos, index));
            }
        }


        public static void Captures(List<Action.Action> list, int pos)
        {
            var piece = PieceOn(pos);
            var color = piece.Color;
            var moveRange = piece.AttackRange;
            var (rank, file) = RankFileOf(pos);
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, moveRange))
            {
                MakeCapture(rankOff, fileOff);
            }
            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank - 1, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank - 1, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank + 1, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank + 1, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange + 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
            }
            return;
            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                if (piece == null || piece.Color == color) return;
                list.Add(new NormalCapture(pos, index));
            }
        }
    }
}