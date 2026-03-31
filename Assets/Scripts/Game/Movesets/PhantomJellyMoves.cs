using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class PhantomJellyMoves //Tentacle moves
    {
        public static int Quiets(List<Action.Action> list, int pos, bool isPlayer)
        {
            var file = FileOf(pos);
            var rank = RankOf(pos);
            var caller = PieceOn(pos);
            var range = caller.GetMoveRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
                MakeMove(rankOff, fileOff);

            int[] dr = { -1, -2, -2, -1, 1, 2, 2, 1 };
            int[] dc = { -2, -1, 1, 2, 2, 1, -1, -2 };

            foreach (var rankOffset in dr)
            foreach (var fileOffset in dc)
            {
                var rankOff = rank + rankOffset;
                var fileOff = file + fileOffset;

                MakeMove(rankOff, fileOff);
            }

            return 30 + 5 * range;

            void MakeMove(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);

                if (piece == null && IsActive(index))
                {
                    list.Add(new NormalMove(caller, index));
                }
            }
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
                    list.Add(new NormalCapture(caller, piece));
                }
                else if (piece != null)
                {
                    if (piece.Color == color ||
                        Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                        return;
                    list.Add(new NormalCapture(caller, piece));
                }
            }
        }
    }
}