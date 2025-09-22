using System.Collections.Generic;
using Unity.Burst;
using static Game.Common.BoardUtils;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), BurstCompile]
    public static class MoveEnumerators
    {
        public static IEnumerable<(int, int)> Up(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && --startRank >= 0) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> Down(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && VerifyUpperBound(++startRank)) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> Left(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && --startFile >= 0) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> Right(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && VerifyUpperBound(++startFile)) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> UpLeft(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && --startRank >= 0 && --startFile >= 0) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> UpRight(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && --startRank >= 0 && VerifyUpperBound(++startFile)) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> DownLeft(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && VerifyUpperBound(++startRank) && --startFile >= 0) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> DownRight(int startRank, int startFile, int maxRange)
        {
            while (--maxRange >= 0 && VerifyUpperBound(++startRank) && VerifyUpperBound(++startFile)) yield return (startRank, startFile);
        }
        
        public static IEnumerable<(int, int)> Around(int startRank, int startFile, int radius)
        {
            var rankTo = startRank - radius;
            if (rankTo >= 0)
                for (var tFileTo = startFile - radius; tFileTo <= startFile + radius; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    yield return (rankTo, tFileTo);
                }

            //Down
            rankTo = startRank + radius;
            if (rankTo < 40)
                for (var tFileTo = startFile - radius; tFileTo <= startFile + radius - 1; tFileTo++)
                {
                    if (!VerifyBounds(tFileTo)) continue;
                    yield return (rankTo, tFileTo);
                }

            //Left
            var fileTo = startFile - radius;
            if (fileTo >= 0)
                for (var tRankTo = startRank - radius + 1; tRankTo <= startRank + radius - 1; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    yield return (tRankTo, fileTo);
                }

            //Right
            fileTo = startFile + radius;
            if (fileTo < 40)
                for (var tRankTo = startRank - radius + 1; tRankTo <= startRank + radius; tRankTo++)
                {
                    if (!VerifyBounds(tRankTo)) continue;
                    yield return (tRankTo, fileTo);
                }
        }
        
        public static IEnumerable<(int, int)> AroundUntil(int startRank, int startFile, int radius)
        {
            for (var cr = 1; cr <= radius; cr++)
            {
                foreach (var (x, y) in Around(startRank, startFile, cr))
                {
                    yield return (x, y);
                }
            
            }
        }
        
        public static IEnumerable<(int, int)> KnightMovement(int startRank, int startFile, int radius)
        {
            var rankTo = startRank - radius;
            if (rankTo >= 0)
            {
                yield return (rankTo, startFile - radius + 1);
                if (radius >= 2)
                {
                    yield return (rankTo, startFile + radius - 1);
                }
            }

            //Down
            rankTo = startRank + radius;
            if (VerifyUpperBound(rankTo))
            {
                yield return (rankTo, startFile - radius + 1);
                if (radius >= 2)
                {
                    yield return (rankTo, startFile + radius - 1);
                }
            }

            //Left
            var fileTo = startFile - radius;
            if (fileTo >= 0)
            {
                yield return (startRank - radius + 1, fileTo);
                if (radius >= 2)
                {
                    yield return (startRank + radius - 1, fileTo);
                }
            }

            //Right
            fileTo = startFile + radius;
            if (VerifyUpperBound(fileTo))
            {
                yield return (startRank - radius + 1, fileTo);
                if (radius >= 2)
                {
                    yield return (startRank + radius - 1, fileTo);
                }
            }
        }
    }
}