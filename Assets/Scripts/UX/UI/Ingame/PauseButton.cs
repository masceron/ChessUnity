using UnityEngine;
using Game.Managers;
using TMPro;
using UnityEngine.UI;
using Game.Common;


namespace UX.UI.Ingame
{
    public class PauseButton : Singleton<PauseButton>
    {
        public static bool isPause = false;
        [SerializeField] private Image targetGraphic;
        void Start()
        {
            gameObject.SetActive(false);
        }
        public void OnClick()
        {
            isPause = !isPause;
            targetGraphic.color = isPause ? UnityEngine.Color.yellow : UnityEngine.Color.white;
        }
        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}