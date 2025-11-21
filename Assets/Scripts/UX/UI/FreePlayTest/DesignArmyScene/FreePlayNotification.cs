using UnityEngine;
using Game.Common;
using UnityEngine.UI;
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
            this.gameObject.SetActive(false);
        }
        public void Open()
        {
            this.gameObject.SetActive(true);
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