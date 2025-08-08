using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Moves
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class PufferfishMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
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
        }
    }
}