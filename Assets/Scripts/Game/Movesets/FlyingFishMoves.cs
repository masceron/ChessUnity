using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingFishMoves : BaseMovePattern
    {
        /*public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var (rank, file) = RankFileOf(pos);
            
            var board = PieceBoard();
            var active = ActiveBoard();
            var effectiveMoveRange = PieceOn(pos).GetMoveRange(ref index);

            for (var rankTo = rank - effectiveMoveRange; rankTo <= rank + effectiveMoveRange; rankTo += effectiveMoveRange)
            {
                if (!VerifyBounds(rankTo)) continue;
                for (var fileTo = file - effectiveMoveRange; fileTo <= file + effectiveMoveRange; fileTo += effectiveMoveRange)
                {
                    if (!VerifyBounds(fileTo)) continue;
                    if (rankTo == rank && fileTo == file) continue;
                    var posTo = IndexOf(rankTo, fileTo);

                    if (board[posTo] == null && active[posTo])
                    {
                        list.Add(new NormalMove(pos, posTo));
                    }
                }
            }
        }
        
        public static void Captures(List<Action.Action> list, int Pos)
        {
            var board = PieceBoard();
            var p = PieceOn(Pos);
            var color = p.Color;
            var attackRange = p.AttackRange;
            
            var ver1 = PushWhite(Pos) * attackRange;
            var ver2 = PushBlack(Pos) * attackRange;

            if (VerifyUpperIndex(ver1) && board[ver1] != null && board[ver1].Color != color)
            {
                list.Add(new NormalCapture(Pos, ver1));
            }
            
            if (ver2 > 0 && ver2 < board.Length && board[ver2] != null && board[ver2].Color != color)
            {
                list.Add(new NormalCapture(Pos, ver2));
            }

            var (rank, file) = RankFileOf(Pos);
                
            var fileOff1 = file - attackRange;
            var fileOff2 = file + attackRange;

            if (fileOff1 > 0)
            {
                var hoz1 = IndexOf(rank, fileOff1);
                
                if (board[hoz1] != null && board[hoz1].Color != color)
                {
                    list.Add(new NormalCapture(Pos, hoz1));
                }
            }

            if (!VerifyUpperBound(fileOff2)) return;
            
            var hoz2 = IndexOf(rank, fileOff2);
            if (board[hoz2] != null && board[hoz2].Color != color)
            {
                list.Add(new NormalCapture(Pos, hoz2));
            }
        }*/

        public override List<int> GenerateBaseMovePattern(int makerPos)
        {
            return GenerateFlyingFishMovePattern(makerPos);
        }

        private List<int> GenerateFlyingFishMovePattern(int makerPos)
        {
            var caller = PieceOn(makerPos);
            var positions = new List<int>();
            var push = caller.Color ? +1 : -1;
            var (rank, file) = RankFileOf(makerPos);

            positions.Add(IndexOf(rank + push * 2, file + 2));
            positions.Add(IndexOf(rank + push * 2, file));
            positions.Add(IndexOf(rank + push * 2, file - 2));

            positions.Add(IndexOf(rank, file + 2));
            positions.Add(IndexOf(rank, file - 2));

            positions.Add(IndexOf(rank - push * 2, file + 2));
            positions.Add(IndexOf(rank - push * 2, file));
            positions.Add(IndexOf(rank - push * 2, file - 2));

            return positions;
        }

        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var moveRange = PieceOn(pos).GetMoveRange(ref index);
            var basePattern = new HashSet<int>(new FlyingFishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, moveRange, forCapture: false, isPlayer: isPlayer);

            return 15 + 5 * moveRange;
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var attackRange = PieceOn(pos).AttackRange;
            var basePattern = new HashSet<int>(new FlyingFishMoves().GenerateBaseMovePattern(pos));
            AddToPatternMoves(list, basePattern, pos, attackRange, forCapture: true, isPlayer: isPlayer);
            return 15 + 5 * attackRange;
        }
    }
}