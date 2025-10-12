using Game.Common;
using UnityEngine;

namespace UX.UI.Ingame
{
    public enum IngameSubmenus
    {
        ChrysosShop,
        ThalassosResurrector,
        DeathDefianceUI,
        DormantFossilUI,

    }

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIHolder: Singleton<UIHolder>
    {
        [SerializeField] private UDictionary<IngameSubmenus, GameObject> submenus;

        public GameObject Get(IngameSubmenus uiName)
        {
            return submenus[uiName];
        }
    }
}