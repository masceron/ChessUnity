using System.Collections.Generic;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class Numerals
    {
        private static readonly List<string> RomanNumerals = new() { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        private static readonly List<int> NumeralCutoffs = new() { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

        public static string ToRomanNumeral(int number)
        {
            var romanNumeral = string.Empty;
            while (number > 0)
            {
                var index = NumeralCutoffs.FindIndex(x => x <= number);
                number -= NumeralCutoffs[index];
                romanNumeral += RomanNumerals[index];
            }
            return romanNumeral;
        }
    }
}