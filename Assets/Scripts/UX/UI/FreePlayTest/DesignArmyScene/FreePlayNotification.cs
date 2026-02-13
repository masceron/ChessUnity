using Game.Common;
using TMPro;
using UnityEngine;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlayNotification : Singleton<FreePlayNotification>
    {
        public TMP_Text armyNameTmp;

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Initialize for FreePlayNoti");
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Save()
        {
            if (!string.IsNullOrWhiteSpace(armyNameTmp.text))
            {
                FPArmyDesign.Ins.army.Name = armyNameTmp.text;
                FPArmyDesign.Ins.Save();
            }
        }
    }
}