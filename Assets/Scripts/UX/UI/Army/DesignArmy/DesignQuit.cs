using DG.Tweening;
using UnityEngine;

namespace UX.UI.Army.DesignArmy
{
    public class DesignQuit: MonoBehaviour
    {
        [SerializeField] private RectTransform menuRect;

        public void Cancel()
        {
            gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            menuRect.rotation = new Quaternion
            {
                eulerAngles = new Vector3(90, 0, 0)
            };
            menuRect.DORotate(new Vector3(0, 0, 0), 0.15f);
        }

        public void Quit()
        {
            gameObject.SetActive(false);
            UIManager.Ins.Load(CanvasID.Followers);
        }
    }
}