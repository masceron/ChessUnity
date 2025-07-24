using System;
using Game.Board.Piece.PieceLogic;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class Pathfinder
    {
        private static float Crawl(float start, float finish, float value)
        {
            if (Math.Abs(start - finish) < 0.0001) return 1;
            if (Math.Sign(finish - start) != Math.Sign(finish - value)) return 1;
            return (value - start) / (finish - start);
        }

        public static Vector2Int LineBlocker(Vector2Int first, Vector2Int second, PieceLogic[] board)
        {
            var firstBlocker = -Vector2Int.one;
            
            var x1 = first.x + 0.5f;
            var y1 = first.y + 0.5f;
            var x2 = second.x + 0.5f;
            var y2 = second.y + 0.5f;
            
            float xDirection = x2 > x1 ? 1 : -1;
            var xCurrent = (float) Math.Floor(x1);
            var xNext = x2 > x1 ? xCurrent + 1 : xCurrent;
            var xProgress = Crawl(x1, x2, xNext);
            
            float yDirection = y2 > y1 ? 1 : -1;
            var yCurrent = (float) Math.Floor(y1);
            var yNext = y2 > y1 ? yCurrent + 1 : yCurrent;
            var yProgress = Crawl(y1, y2, yNext);
            
            while (xProgress < 1 || yProgress < 1) {
                var shouldMoveX = xProgress <= yProgress;
                var shouldMoveY = yProgress <= xProgress;
	
                if (shouldMoveX) {
                    xCurrent += xDirection;
                    xNext += xDirection;
                    xProgress = Crawl(x1, x2, xNext);
                }
                if (shouldMoveY) {
                    yCurrent += yDirection;
                    yNext += yDirection;
                    yProgress = Crawl(y1, y2, yNext);
                }

                if (board[IndexOf((int)xCurrent, (int)yCurrent)] != null)
                {
                    firstBlocker = new Vector2Int((int)xCurrent, (int)yCurrent);
                    break;
                }

            }

            if (!firstBlocker.Equals(second)) return firstBlocker;
            return -Vector2Int.one;
        }
    }
}