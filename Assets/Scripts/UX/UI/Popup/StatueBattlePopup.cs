using Game.Statue;
using UnityEngine;
using UnityEngine.UI;
using UX.UI.Loader;

namespace UX.UI.Popup
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StatueBattlePopup : MonoBehaviour
    {
        [Header("UI References")] [SerializeField]
        private Button fightButton;

        [SerializeField] private Button skipButton;

        private Statue currentStatue;

        private void Awake()
        {
            if (fightButton != null) fightButton.onClick.AddListener(OnFightClicked);

            if (skipButton != null) skipButton.onClick.AddListener(OnSkipClicked);
        }

        private void OnDestroy()
        {
            if (fightButton != null) fightButton.onClick.RemoveListener(OnFightClicked);

            if (skipButton != null) skipButton.onClick.RemoveListener(OnSkipClicked);
        }

        public void Initialize(Statue statue)
        {
            currentStatue = statue;
        }

        private void OnFightClicked()
        {
            if (currentStatue != null)
            {
                var blackPieces = currentStatue.GetBlackPieceConfigs();
                SceneLoader.LoadStatueBattle(blackPieces);
            }
        }

        private void OnSkipClicked()
        {
            gameObject.SetActive(false);
        }
    }
}