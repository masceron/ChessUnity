using UnityEngine;
using UX.UI.Army.DesignArmy;
using UX.UI.FreePlayTest.RegionalRealmScene;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FinishDesignArmy : MonoBehaviour
    {
        [SerializeField] private FreePlayNotification notification;
        public void OnClick()
        {
            if (!string.IsNullOrWhiteSpace(FreePlayArmyDesign.Ins.army.Name))
            {
                Debug.Log($"Save: {FreePlayArmyDesign.Ins.army.Name}");
                FreePlayArmyDesign.Ins.Save();
            }
            else
            {
                notification.Open();
            }
        }
    }
}