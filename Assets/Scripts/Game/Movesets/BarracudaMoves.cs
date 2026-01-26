using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BarracudaMoves : BaseMovePattern // Medium Predator Move.cs
    {
       /* public static void Quiets(List<Action.Action> list, int pos)
        {
            var (rank, pieceFile) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange(ref index);
            var color = piece.Color;
            
            for (var i = 1; i <= moveRange; i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeMove(rank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeMove(rank + i, file, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i; tRank++)
                {
                    MakeMove(tRank, pieceFile + i, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i - 1; tRank++)
                {
                    MakeMove(tRank, pieceFile - i, i);
                }
            }

            return;

            void MakeMove(int tRank, int file, int distance)
            {
                if (!VerifyBounds(tRank) || !VerifyBounds(file)) return;

                var tpos = IndexOf(tRank, file);
                if (!IsActive(tpos)) return;

                var pieceOn = PieceOn(tpos);
                if (pieceOn != null) return;

                if (distance > moveRange) return;
                
                if (distance == moveRange)
                {
                    if (!color)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                    
                list.Add(new NormalMove(pos, tpos));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, pieceFile) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.AttackRange;
            var color = piece.Color;
            
            for (var i = 1; i <= moveRange; i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeCapture(rank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeCapture(rank + i, file, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i; tRank++)
                {
                    MakeCapture(tRank, pieceFile + i, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i - 1; tRank++)
                {
                    MakeCapture(tRank, pieceFile - i, i);
                }
            }

            return;

            void MakeCapture(int tRank, int file, int distance)
            {
                if (!VerifyBounds(tRank) || !VerifyBounds(file)) return;
            
                var tpos = IndexOf(tRank, file);
                if (!IsActive(IndexOf(tRank, file))) return;
            
                var pieceOn = PieceOn(tpos);

                if (distance > moveRange || pieceOn == null || pieceOn.Color == color) return;
                
                if (distance == moveRange)
                {
                    if (!color)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (pieceOn.Color != color)
                    list.Add(new NormalCapture(pos, tpos));
            }
        }*/

        override public List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateBarracudaMovePattern(makerPos);
        }

        private List<int> GenerateBarracudaMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file + 2));
            positions.Add(IndexOf(rank + push * 2, file + 1));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 2, file - 1));
            positions.Add(IndexOf(rank + push * 2, file - 2));

            positions.Add(IndexOf(rank + push * 1, file + 2));
            positions.Add(IndexOf(rank + push * 1, file + 1));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file - 1));
            positions.Add(IndexOf(rank + push * 1, file - 2));

            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));

            positions.Add(IndexOf(rank - push * 1, file + 1));
            positions.Add(IndexOf(rank - push * 1, file));
            positions.Add(IndexOf(rank - push * 1, file - 1));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new BarracudaMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, excludeEmptyTile: excludeEmptyTile);
            return 30 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new BarracudaMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, excludeEmptyTile: excludeEmptyTile);
            return 30 + 5 * attackRange;
        }
    }
}