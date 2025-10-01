using Game.Managers;
using Game.Relics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UX.UI.Ingame
{
    public delegate void VoidDelegates();

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GameActions: MonoBehaviour
    {
        [SerializeField] private CanvasGroup gameAction;
        [SerializeField] private Button relic;
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
            switch (whiteRelic.Type)
            {
                case RelicType.CommonPearl:
                    //_whiteRelic = new CommonPearl(whiteRelic.Color);
                    break;
                case RelicType.BlackPearl:
                    //_whiteRelic = new BlackPearl(whiteRelic.Color);
                    break;
                case RelicType.EyeOfMimic:
                    _whiteRelic = new EyeOfMimic(whiteRelic);
                    break;
            }
            switch (blackRelic.Type)
            {
                case RelicType.CommonPearl:
                    //_blackRelic = new CommonPearl(blackRelic.Color);
                    break;
                case RelicType.BlackPearl:
                    //_blackRelic = new BlackPearl(blackRelic.Color);
                    break;
                case RelicType.EyeOfMimic:
                    _blackRelic = new EyeOfMimic(blackRelic);
                    break;
            }
        }

        public void DisableGameInteractions()
        {
            gameAction.interactable = false;
        }

        public void EnableGameInteractions()
        {
            gameAction.interactable = true;
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
        }
    }
}