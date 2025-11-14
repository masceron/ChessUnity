using System;
using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ElectricEelMoves : BaseMovePattern
    {
        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GeneratePillarMovePattern(makerPos);
        }

        private List<int> GeneratePillarMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            //int push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + 2, file + 2));
            positions.Add(IndexOf(rank + 2, file + 1));
            positions.Add(IndexOf(rank + 2, file));
            positions.Add(IndexOf(rank + 2, file - 1));
            positions.Add(IndexOf(rank + 2, file - 2));

            positions.Add(IndexOf(rank + 1, file + 1));
            positions.Add(IndexOf(rank + 1, file));
            positions.Add(IndexOf(rank + 1, file - 1));

            positions.Add(IndexOf(rank, file + 1));
            positions.Add(IndexOf(rank, file - 1));

            positions.Add(IndexOf(rank - 1, file + 1));
            positions.Add(IndexOf(rank - 1, file));
            positions.Add(IndexOf(rank - 1, file - 1));

            positions.Add(IndexOf(rank - 2, file + 2));
            positions.Add(IndexOf(rank - 2, file + 1));
            positions.Add(IndexOf(rank - 2, file));
            positions.Add(IndexOf(rank - 2, file - 1));
            positions.Add(IndexOf(rank - 2, file - 2));

            return positions;
        }

        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new ElectricEelMoves().GenerateBaseMovePattern(pos));
            BaseMovePattern.AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false);
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new ElectricEelMoves().GenerateBaseMovePattern(pos));
            BaseMovePattern.AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true);
        }
    }
}