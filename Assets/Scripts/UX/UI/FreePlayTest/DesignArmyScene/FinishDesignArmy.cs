using UnityEngine;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FinishDesignArmy : MonoBehaviour
    {
        [SerializeField] private FreePlayNotification notification;
        public void OnClick()
        {
            if (!string.IsNullOrWhiteSpace(FPArmyDesign.Ins.army.Name))
            {
                Debug.Log($"Save: {FPArmyDesign.Ins.army.Name}");
                FPArmyDesign.Ins.Save();
            }
            else
            {
                notification.Open();
            }
        }
    }
}