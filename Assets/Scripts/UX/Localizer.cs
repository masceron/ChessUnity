using TMPro;
using UnityEngine.Localization;

namespace UX
{
    public static class Localizer
    {
        private static readonly LocalizedString Localized = new();

        public static void SetText(TMP_Text text, string table, string key)
        {
            Localized.SetReference(table, key);
            text.text = Localized.GetLocalizedString();
        }

        public static string GetText(string table, string key, object[] arguments)
        {
            Localized.SetReference(table, key);
            return Localized.GetLocalizedString(arguments);
        }
    }
}