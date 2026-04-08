using Game.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Common
{
    public class UIHolder: Singleton<UIHolder>
    {
        [SerializeField] private UDictionary<InGameMenuType, VisualTreeAsset> menuTemplates;

        public bool Get(InGameMenuType type, out VisualTreeAsset asset)
        {
            try
            {
                asset = menuTemplates[type];
                return true;
            }
            catch
            {
                asset = null;
                return false;
            }
        }
    }
}