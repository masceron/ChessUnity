using TMPro;
using UnityEngine.Localization;

namespace UX
{
    public static class Localize
    {
        private static readonly LocalizedString Localized = new();
        
        public static void SetText(TMP_Text text, string table, string key)
        {
            Localized.SetReference(table, key);
            text.text = Localized.GetLocalizedString();
        }
    }
}