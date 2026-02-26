using System;
using Unity.Burst;
using static Game.Common.BoardUtils;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [BurstCompile]
    public static class Pathfinder
    {
        [BurstCompile]
        private static float Crawl(float start, float finish, float value)
        {
            if (Math.Abs(start - finish) < 0.0001) return 1;
            if (Math.Sign(finish - start) != Math.Sign(finish - value)) return 1;
            return (value - start) / (finish - start);
        }

        public static (int, int) LineBlocker(int first1, int first2, int second1, int second2)
        {
            var board = PieceBoard();
            var activeBoard = ActiveBoard();

            var firstBlockerX = -1;
            var firstBlockerY = -1;

            var x1 = first1 + 0.5f;
            var y1 = first2 + 0.5f;
            var x2 = second1 + 0.5f;
            var y2 = second2 + 0.5f;

            float xDirection = x2 > x1 ? 1 : -1;
            var xCurrent = (float)Math.Floor(x1);
            var xNext = x2 > x1 ? xCurrent + 1 : xCurrent;
            var xProgress = Crawl(x1, x2, xNext);

            float yDirection = y2 > y1 ? 1 : -1;
            var yCurrent = (float)Math.Floor(y1);
            var yNext = y2 > y1 ? yCurrent + 1 : yCurrent;
            var yProgress = Crawl(y1, y2, yNext);

            while (xProgress < 1 || yProgress < 1)
            {
                var shouldMoveX = xProgress <= yProgress;
                var shouldMoveY = yProgress <= xProgress;

                if (shouldMoveX)
                {
                    xCurrent += xDirection;
                    xNext += xDirection;
                    xProgress = Crawl(x1, x2, xNext);
                }

                if (shouldMoveY)
                {
                    yCurrent += yDirection;
                    yNext += yDirection;
                    yProgress = Crawl(y1, y2, yNext);
                }

                var tmpIdx = IndexOf((int)xCurrent, (int)yCurrent);
                if (board[tmpIdx] == null && activeBoard[tmpIdx]) continue;

                firstBlockerX = (int)xCurrent;
                firstBlockerY = (int)yCurrent;
                break;
            }

            if (!(firstBlockerX == second1 && firstBlockerY == second2)) return (firstBlockerX, firstBlockerY);
            return (-1, -1);
        }
    }
}