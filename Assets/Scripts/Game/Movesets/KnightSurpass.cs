using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class KnightSurpass
    {
        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var (rank, file) = RankFileOf(pos);
            var caller = PieceOn(pos);
            var maxRange = caller.GetMoveRange(ref index);

            foreach (var (rankOff, fileOff) in MoveEnumerators.KnightMovement(rank, file, maxRange))
            {
                MakeMove(rankOff, fileOff);
            }
            
            return 15 + 3 * maxRange;
            
            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                
                var piece = PieceOn(index);
                if (piece != null ||
                    Distance(pos, index) != maxRange)
                    return;
                list.Add(new NormalMove(pos, index));
            }
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var (rank, file) = RankFileOf(pos);
            var caller = PieceOn(pos);
            var color = caller.Color;
            var maxRange = caller.AttackRange;
            
            foreach (var (rankOff, fileOff) in MoveEnumerators.KnightMovement(rank, file, maxRange))
            {
                MakeCapture(rankOff, fileOff);
            }

            return 15 + 3 * maxRange;
            
            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                
                if (piece == null && !isPlayer)
                {
                    list.Add(new NormalCapture(pos, index));
                    return;
                }
                else if (piece != null)
                {
                    if (piece.Color == color ||
                    Distance(pos, index) != maxRange)
                        return;
                    list.Add(new NormalCapture(pos, index));
                }
            }
        }
    }
}