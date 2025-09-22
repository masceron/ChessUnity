using PrimeTween;
using TMPro;
using UnityEngine;

namespace UX.UI.Army.DesignArmy
{

    public enum DesignNotifications
    {
        Quit,
        EmptyName,
        Overwrite
    }
    
    public class DesignNotification: MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private RectTransform menuRect;
        [SerializeField] private TMP_Text no;
        [SerializeField] private TMP_Text yes;
        private DesignNotifications stage;

        public void OnEnable()
        {
            menuRect.rotation = new Quaternion
            {
                eulerAngles = new Vector3(90, 0, 0)
            };
            
            Tween.Rotation(menuRect, Vector3.zero, 0.15f);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open(DesignNotifications ntf)
        {
            stage = ntf;
            gameObject.SetActive(true);
            
            switch (ntf)
            {
                case DesignNotifications.Quit:
                    no.transform.parent.gameObject.SetActive(true);
                    Localizer.SetText(text, "design", "design_quit");
                    Localizer.SetText(no, "common", "cancel");
                    Localizer.SetText(yes, "common", "quit");
                    break;
                case DesignNotifications.EmptyName:
                    no.transform.parent.gameObject.SetActive(false);
                    Localizer.SetText(text, "design", "design_noname");
                    Localizer.SetText(yes, "common", "ok");
                    break;
                case DesignNotifications.Overwrite:
                    no.transform.parent.gameObject.SetActive(true);
                    Localizer.SetText(text, "design", "design_overwrite");
                    Localizer.SetText(no, "common", "cancel");
                    Localizer.SetText(yes, "common", "overwrite");
                    break;
            }
        }
        
        public void No()
        {
            gameObject.SetActive(false);
        }

        public void Yes()
        {
            switch (stage)
            {
                case DesignNotifications.Quit:
                    gameObject.SetActive(false);
                    UIManager.Ins.Load(CanvasID.Followers);
                    break;
                case DesignNotifications.EmptyName:
                    gameObject.SetActive(false);
                    break;
                case DesignNotifications.Overwrite:
                    ArmyDesign.Ins.Save();
                    break;
            }
            
        }
    }
}