using UnityEngine;
using Game.Common;
using TMPro;

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
                FreePlayArmyDesign.Ins.army.Name = armyNameTmp.text;
                FreePlayArmyDesign.Ins.Save();
            }
            
        }
    }
}