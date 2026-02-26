using System.Collections.Generic;
using Game.Action.Captures;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class PhantomJellyMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, bool isPlayer)
        {
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var range = caller.GetAttackRange();

            var color = caller.Color;

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
                MakeCapture(rankOff, fileOff);

            int[] dr = { -1, -2, -2, -1, 1, 2, 2, 1 };
            int[] dc = { -2, -1, 1, 2, 2, 1, -1, -2 };

            foreach (var rankOffset in dr)
            foreach (var fileOffset in dc)
            {
                var rankOff = rank + rankOffset;
                var fileOff = file + fileOffset;

                MakeCapture(rankOff, fileOff);
            }

            return 30 + 5 * range;

            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);

                if (piece == null && !isPlayer)
                {
                    list.Add(new NormalCapture(pos, index));
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