using Game.Common;
using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [RequireComponent(typeof(ArmyDesign))]
    public class FreePlayArmyDesign : Singleton<FreePlayArmyDesign>
    {
        private ArmyDesign armyDesign;
        public Transform nextButton;
        void Start()
        {
            armyDesign = GetComponent<ArmyDesign>();
            nextButton.gameObject.SetActive(false);
        }
        public void Load(int size)
        {
            armyDesign.Load(size, null);
            nextButton.gameObject.SetActive(true);
        }
    }
}