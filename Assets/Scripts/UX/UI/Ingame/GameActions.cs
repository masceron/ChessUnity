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

        private VoidDelegates endTurn;

        public void Load(VoidDelegates end)
        {
            endTurn = end;
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
    }
}