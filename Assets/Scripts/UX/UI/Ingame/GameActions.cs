using UnityEngine;
using Game.Action;
using Game.Common;
using Game.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameActions: MonoBehaviour
    {
        [SerializeField] private CanvasGroup gameAction;
        [SerializeField] private Button relic;
        [SerializeField] private Button skip;

        private void OnEnable()
        {
            relic.onClick.AddListener(PressRelic);
        }
        private void OnDisable()
        {
            relic.onClick.RemoveListener(PressRelic);
        }

        public void DisableGameInteractions()
        {
            gameAction.interactable = false;
        }

        public void EnableGameInteractions()
        {
            gameAction.interactable = true;
            UpdateRelic();
        }

        public void UpdateRelic()
        {
            var relicLogic = BoardUtils.GetRelicOf(MatchManager.Ins.GameState.OurSide);
            if (relicLogic == null) return;
            
            relicCooldownText.gameObject.SetActive(relicLogic.currentCooldown != 0);
            relic.interactable = relicLogic.currentCooldown == 0;
            relicCooldownText.text = relicLogic.currentCooldown.ToString();
        }

        public void ClickEndTurn()
        {
            BoardViewer.Ins.ExecuteAction(new SkipTurn());
        }
        
        public void PressEndTurn(InputAction.CallbackContext context)
        {
            if (!context.performed || !skip.interactable) return;
            BoardViewer.Ins.ExecuteAction(new SkipTurn());
        }

        private void PressRelic()
        {
            if (!relic.interactable) return;
            BoardUtils.GetRelicOf(MatchManager.Ins.GameState.OurSide)?.Activate();
        }
    }
}