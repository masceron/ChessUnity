using Game.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Ingame
{
    public class PauseButton : Singleton<PauseButton>
    {
        public static bool isPause;
        [SerializeField] private Image targetGraphic;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        public void OnClick()
        {
            isPause = !isPause;
            targetGraphic.color = isPause ? Color.yellow : Color.white;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}