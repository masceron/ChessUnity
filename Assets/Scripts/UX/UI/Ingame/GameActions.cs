using Game.Managers;
using Game.Relics;
using Game.Relics.RottingScythe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace UX.UI.Ingame
{
    public delegate void VoidDelegates();

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameActions: MonoBehaviour
    {
        [SerializeField] private CanvasGroup gameAction;
        [SerializeField] private Button relic;
        [SerializeField] private TMP_Text relicCooldownText;
        [SerializeField] private Button skip;

        private RelicLogic _whiteRelic;
        private RelicLogic _blackRelic;

        private VoidDelegates endTurn;

        public void Load(VoidDelegates end)
        {
            endTurn = end;
        }

        private void OnEnable()
        {
            relic.onClick.AddListener(PressRelic);
        }
        private void OnDisable()
        {
            relic.onClick.RemoveListener(PressRelic);
        }

        public void LoadRelic(RelicConfig whiteRelic, RelicConfig blackRelic)
        {
            _whiteRelic = GetRelicLogicByConfig(whiteRelic);
            _blackRelic = GetRelicLogicByConfig(blackRelic);
        }

        private RelicLogic GetRelicLogicByConfig(RelicConfig cfg)
        {
            RelicLogic rl = cfg.Type switch 
            { 
                // RelicType.EyeOfMimic => new EyeOfMimic(cfg),
                RelicType.RottingScythe => new RottingScythe(cfg),
                RelicType.EyeOfMimic => new EyeOfMimic(cfg),
                _ => null
            };
            return rl;
        }

        public void DisableGameInteractions()
        {
            gameAction.interactable = false;
            relicCooldownText.gameObject.SetActive(false);
        }

        public void EnableGameInteractions()
        {
            gameAction.interactable = true;
            UpdateRelic();
        }

        public void UpdateRelic()
        {
            if (MatchManager.Ins.GameState.OurSide)
            {
                relicCooldownText.gameObject.SetActive(_blackRelic.currentCooldown != 0);
                relic.interactable = _blackRelic.currentCooldown == 0;
                relicCooldownText.text = _blackRelic.currentCooldown.ToString();
            }
            else
            {
                relicCooldownText.gameObject.SetActive(_whiteRelic.currentCooldown != 0);
                relic.interactable = _whiteRelic.currentCooldown == 0;
                relicCooldownText.text = _whiteRelic.currentCooldown.ToString();
            }
        }
        
        public void PressEndTurn(InputAction.CallbackContext context)
        {
            if (!context.performed || !skip.interactable) return;
            endTurn();
        }

        public void PressRelic()
        {
            if (!relic.interactable) return;
            if (MatchManager.Ins.GameState.OurSide == false)
            {
                _whiteRelic?.Activate();
                
            }
            else
            {
                _blackRelic?.Activate();
                
            }
        }

        public void PassTurn()
        {
            _whiteRelic?.PassTurn();
            _blackRelic?.PassTurn();
            UpdateRelic();
        }
    }
}