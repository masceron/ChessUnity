using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BluffingMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange(ref index);
            Debug.Log(moveRange);
            var color = piece.Color;
            var push = color ? 1 : -1;

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange - 1))
            {
                MakeMove(rankOff, fileOff);
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange - 1))
            {
                MakeMove(rankOff, fileOff);
            }
            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange - 1))
                {
                    MakeMove(rankOff, fileOff);
                }

                for (var rankOff = rank + push ;rankOff <= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff <= file + (rankOff - rank); fileOff++) 
                    {
                        MakeMove(rankOff, fileOff);
                    }
                }

            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange - 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                
                for (var rankOff = rank + push ; rankOff >= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff >= file + (rankOff - rank); fileOff--) 
                    {
                        MakeMove(rankOff, fileOff);
                    }
                }
            }
            
            return;

            void MakeMove(int rankOff, int fileOff)
            {
                if (!VerifyBounds(rankOff) || !VerifyBounds(fileOff)) return;
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece != null) return;
                list.Add(new NormalMove(pos, index));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.AttackRange;
            var color = piece.Color;
            var push = color ? 1 : -1;

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange - 1))
            {
                MakeCapture(rankOff, fileOff);
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange - 1))
            {
                MakeCapture(rankOff, fileOff);
            }
            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange - 1))
                {
                    MakeCapture(rankOff, fileOff);
                }

                for (var rankOff = rank + push ;rankOff <= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff <= file + (rankOff - rank); fileOff++) 
                    {
                        MakeCapture(rankOff, fileOff);
                    }
                }

            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange - 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                
                for (var rankOff = rank + push ; rankOff >= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff >= file + (rankOff - rank); fileOff--) 
                    {
                        MakeCapture(rankOff, fileOff);
                    }
                }
            }

            return;

            void MakeCapture(int rankOff, int fileOff)
            {
                if (!VerifyBounds(rankOff) || !VerifyBounds(fileOff)) return;
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece == null) return;
                if (piece.Color == color) return;
                list.Add(new NormalCapture(pos, index));
            }
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateBluffingMovePattern(makerPos);
        }

        private List<int> GenerateBluffingMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 1, file));

            positions.Add(IndexOf(rank, file - 1));
            positions.Add(IndexOf(rank, file + 1));

            positions.Add(IndexOf(rank - push * 1, file - 1));
            positions.Add(IndexOf(rank - push * 1, file));
            positions.Add(IndexOf(rank - push * 1, file + 1));

            positions.Add(IndexOf(rank - push * 2, file - 2));
            positions.Add(IndexOf(rank - push * 2, file - 1));
            positions.Add(IndexOf(rank - push * 2, file));
            positions.Add(IndexOf(rank - push * 2, file + 1));
            positions.Add(IndexOf(rank - push * 2, file + 2));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new BluffingMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, isPlayer: isPlayer);

            return 15 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new BluffingMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, isPlayer: isPlayer);
            return 15 + 5 * attackRange;
        }
    }    
}