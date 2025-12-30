using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PufferfishMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos)
        {
            var caller = PieceOn(pos);
            var color = caller.Color;
            var range = caller.GetMoveRange(ref index);
            var (startRank, startFile) = RankFileOf(pos);

            var push = !color ? -1 : 1;

            for (var rank = RankOf(pos) - (range - 1) * push; rank != RankOf(pos) + (range + 1) * push; rank += push)
            {
                if (!VerifyBounds(rank)) continue;
                for (var file = FileOf(pos) - 1; file <= FileOf(pos) + 1; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    var idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;
                    
                    var piece = PieceOn(idx);

                    if (piece != null) continue;
                    if (Pathfinder.LineBlocker(startRank, startFile, rank, file).Item1 != -1) continue;
                    
                    list.Add(new NormalMove(pos, idx));
                }
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var caller = PieceOn(pos);
            var color = caller.Color;
            var range = caller.AttackRange;
            var (startRank, startFile) = RankFileOf(pos);
            
            var push = !color ? -1 : 1;

            for (var rank = RankOf(pos) - (range - 1) * push; rank != RankOf(pos) + (range + 1) * push; rank += push)
            {
                if (!VerifyBounds(rank)) continue;
                for (var file = FileOf(pos) - 1; file <= FileOf(pos) + 1; file++)
                {
                    if (!VerifyBounds(file)) continue;
                    var idx = IndexOf(rank, file);
                    if (!IsActive(idx)) continue;
                    
                    var piece = PieceOn(idx);

                    if (piece == null || piece.Color == color) continue;
                    if (Pathfinder.LineBlocker(startRank, startFile, rank, file).Item1 != -1) continue;
                    
                    list.Add(new NormalCapture(pos, idx));
                }
            }
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GeneratePufferfishMovePattern(makerPos);
        }

        private List<int> GeneratePufferfishMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file - 1));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 2, file + 1));

            positions.Add(IndexOf(rank + push * 1, file - 1));
            positions.Add(IndexOf(rank + push * 1, file));
            positions.Add(IndexOf(rank + push * 1, file + 1));

            positions.Add(IndexOf(rank, file - 1));
            positions.Add(IndexOf(rank, file + 1));

            positions.Add(IndexOf(rank - push * 1, file - 1));
            positions.Add(IndexOf(rank - push * 1, file));
            positions.Add(IndexOf(rank - push * 1, file + 1));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new PufferfishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, excludeEmptyTile: excludeEmptyTile);

            return 20 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).GetAttackRange();
            var basePattern = new HashSet<int>(new PufferfishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, excludeEmptyTile: excludeEmptyTile);
            return 20 + 5 * attackRange;
        }
    }
}