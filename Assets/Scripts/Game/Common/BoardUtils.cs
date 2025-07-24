using System;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class BoardUtils
    {
        public const int MaxLength = 12;
        public const int BoardSize = MaxLength * MaxLength;

        public static int RankOf(int index)
        {
            return index / MaxLength;
        }

        public static int FileOf(int index)
        {
            return index % MaxLength;
        }

        public static (int, int) RankFileOf(int index)
        {
            return (RankOf(index), FileOf(index));
        }

        public static int IndexOf(int rank, int file)
        {
            return rank * MaxLength + file;
        }

        public static bool VerifyUpperBound(int dimension)
        {
            return dimension < MaxLength;
        }

        public static bool VerifyBounds(int dimension)
        {
            return dimension is >= 0 and < MaxLength;
        }

        public static bool VerifyIndex(int index)
        {
            return index is >= 0 and < BoardSize;
        }

        public static bool VerifyUpperIndex(int index)
        {
            return index < BoardSize;
        }
 
        public static int PushWhite(int pos)
        {
            return pos + MaxLength;
        }

        public static int PushBlack(int pos)
        {
            return pos - MaxLength;
        }

        public static int ClampDown(int dimension)
        {
            return Math.Min(dimension, MaxLength - 1);
        }

        public static int ClampUp(int dimension)
        {
            return Math.Max(dimension, 0);
        }

        public static int RowIndex(int row)
        {
            return row * MaxLength;
        }
    }
}