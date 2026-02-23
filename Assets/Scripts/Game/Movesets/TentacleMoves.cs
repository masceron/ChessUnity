using System.Collections.Generic;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    public class TentacleMoves : BaseMovePattern
    {
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateTentacleMovePattern(makerPos);
        }
        
        private List<int> GenerateTentacleMovePattern(int makerPos)
        {
            //TODO: Implement the exact move pattern for TentacleMoves.
            var positions = new List<int>();
            var (rank, file) = RankFileOf(makerPos);
            var piece = PieceOn(makerPos);
            int range = piece.GetMoveRange();

            // 1️⃣ ĐƯỜNG DỌC
            for (int k = 1; k <= range; k++)
            {
                positions.Add(IndexOf(rank + k, file));
                positions.Add(IndexOf(rank - k, file));
            }

            // 2️⃣ ĐƯỜNG CHÉO
            for (int k = 1; k <= range; k++)
            {
                positions.Add(IndexOf(rank + k, file + k));
                positions.Add(IndexOf(rank + k, file - k));
                positions.Add(IndexOf(rank - k, file + k));
                positions.Add(IndexOf(rank - k, file - k));
            }

            // 3️⃣ XÚC TU LỆCH (bám theo trục)
            for (int k = 1; k <= range; k++)
            {
                positions.Add(IndexOf(rank + k, file + 1));
                positions.Add(IndexOf(rank + k, file - 1));
                positions.Add(IndexOf(rank - k, file + 1));
                positions.Add(IndexOf(rank - k, file - 1));

                positions.Add(IndexOf(rank + 1, file + k));
                positions.Add(IndexOf(rank - 1, file + k));
                positions.Add(IndexOf(rank + 1, file - k));
                positions.Add(IndexOf(rank - 1, file - k));
            }

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var moveRange = PieceOn(pos).GetMoveRange();
            var basePattern = new HashSet<int>(new TentacleMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, false, excludeEmptyTile);
            return 30 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool excludeEmptyTile)
        {
            var attackRange = PieceOn(pos).AttackRange();
            var basePattern = new HashSet<int>(new TentacleMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, true, excludeEmptyTile);
            return 30 + 5 * attackRange;
        }
    }
}