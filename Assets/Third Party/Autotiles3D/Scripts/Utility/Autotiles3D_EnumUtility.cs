using System;
using System.Collections.Generic;
using ZLinq;

namespace Third_Party.Autotiles3D.Scripts.Utility
{
    public static class Autotiles3DEnumUtility
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) + 1;
            return (arr.Length == j) ? arr[0] : arr[j];
        }
        public static T Previous<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) - 1;
            return (j == -1) ? arr[^1] : arr[j];
        }

        public static IEnumerable<Enum> GetFlags(this Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).ToArray();
        }

        public static int GetArrayIndexOf<T>(this T src) where T : struct
        {
            var arr = (T[])Enum.GetValues(src.GetType());
            return Array.IndexOf(arr, src);
        }

        public static T GetEnumByArrayIndex<T>(int arrayIndex) where T : struct
        {
            var arr = (T[])Enum.GetValues(typeof(T));
            return arr[arrayIndex];
        }

        public static int GetLength<T>() where T : struct
        {
            return Enum.GetNames(typeof(T)).Length;
        }


    }
}
