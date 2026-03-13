using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class TentacleMoves : BaseMovePattern
    {
        // ====== VECTOR CỐ ĐỊNH ======

        // Pattern gốc (moveRange = 1)
        static readonly (int dr, int df)[] CorePattern =
        {
            // trục chính
            (1, 0), (-1, 0), (0, 1), (0, -1),

            // trục chéo
            (1, 1), (1, -1), (-1, 1), (-1, -1),

            // nhánh lệch
            (1, 2), (1, -2), (-1, 2), (-1, -2),
            (2, 1), (-2, 1), (2, -1), (-2, -1),
        };

        private HashSet<int> GenerateTentacleMovePattern(int pos, int range)
        {
            var result = new HashSet<int>();
            var (rank, file) = RankFileOf(pos);
            
            foreach (var (dr, df) in CorePattern)
            {
                var r = rank + dr;
                var f = file + df;
                if (VerifyBounds(r) && VerifyBounds(f))
                    result.Add(IndexOf(r, f));
            }

            return result;
        }
        

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var range = PieceOn(pos).GetMoveRange();
            var pattern = new TentacleMoves().GenerateTentacleMovePattern(pos, range);

            AddToPatternMoves(list, pattern, pos, range, false, excludeEmptyTile);
            return 30 + 5 * range;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var range = PieceOn(pos).AttackRange();
            var pattern = new TentacleMoves().GenerateTentacleMovePattern(pos, range);

            AddToPatternMoves(list, pattern, pos, range, true, excludeEmptyTile);
            return 30 + 5 * range;
        }

        // Không dùng nữa nhưng phải override
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return new List<int>();
        }
    }
}