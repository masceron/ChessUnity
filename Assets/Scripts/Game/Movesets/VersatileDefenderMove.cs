using System.Collections.Generic;
using static Game.Common.BoardUtils;


namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    /// ******x*****
    /// ****xxxxx****
    /// ****xxOxx****
    /// *****xxx*****
    public class VersatileDefenderMove : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos)
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
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateVersatileDefenderMovePattern(makerPos);
        }

        private List<int> GenerateVersatileDefenderMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file));

            positions.Add(IndexOf(rank + push * 1, file - 2));
            positions.Add(IndexOf(rank + push * 1, file - 1));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file + 1));
            positions.Add(IndexOf(rank + push * 1, file + 2));

            positions.Add(IndexOf(rank, file - 2));
            positions.Add(IndexOf(rank, file - 1));
            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file + 2));

            positions.Add(IndexOf(rank - push * 1, file - 1));
            positions.Add(IndexOf(rank - push * 1, file));
            positions.Add(IndexOf(rank - push * 1, file + 1));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool isPlayer)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new VersatileDefenderMove().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, isPlayer: isPlayer);

            return 40 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new VersatileDefenderMove().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, isPlayer: isPlayer);
            return 40 + 5 * attackRange;
        }
    }
}