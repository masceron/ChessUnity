using System.Linq;
using Game.Save.Army;
using UX.UI.Followers;
using UnityEngine;

namespace UX.UI.FreePlayTest
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FreePlaySavedArmies : SavedArmies
    {
        protected override void Awake()
        {
            base.Awake();
            Load();
        }

    }
}