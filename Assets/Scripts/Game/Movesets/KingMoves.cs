using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class KingMoves
    {
        public static int Quiets(List<Action.Action> list, int pos, ref int index, bool isPlayer)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);

            var effectiveMoveRange = caller.GetMoveRange(ref index);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, effectiveMoveRange))
            {
                MakeMove(rankOff, fileOff);
            }

            return 10 + 10 * effectiveMoveRange;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;

                var piece = PieceOn(index);
                if (piece != null ||
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                list.Add(new NormalMove(pos, index));
            }
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);

            var color = caller.Color;
            var attackRange = caller.AttackRange;

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
            {
                MakeCapture(rankOff, fileOff);
            }

            return 10 + 10 * attackRange;

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
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                        return;
                    list.Add(new NormalCapture(pos, index));
                }

            }
        }
    }
}