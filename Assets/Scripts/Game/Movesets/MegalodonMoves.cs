using System.Collections.Generic;
using Game.Action.Captures;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class MegalodonMoves
    {
        public static int Captures(List<Action.Action> list, int pos)
        {
            var caller = PieceOn(pos);
            var color = caller.Color;
            var range = caller.AttackRange;
            var (startRank, startFile) = RankFileOf(pos);
            
            var push = !color ? -1 : 1;
            
            for (var rank = RankOf(pos) - (range - 1) * push; rank != RankOf(pos) + (range + 1) * push; rank += push)
            {
                if (!VerifyBounds(rank)) continue;
                for (var file = FileOf(pos) - range * push; file != FileOf(pos) + range * push; file += push)
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

            return 10 + 10 * range;
        }
    }
    
    
}