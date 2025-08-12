using Game.Common;
using UnityEngine;

namespace UX.UI.Ingame
{
    public enum IngameSubmenus
    {
        ChrysosShop,
        ThalassosResurrector
    }
    public class UIHolder: Singleton<UIHolder>
    {
        [SerializeField] private UDictionary<IngameSubmenus, GameObject> submenus;

        public GameObject Get(IngameSubmenus uiName)
        {
            return submenus[uiName];
        }
    }
}