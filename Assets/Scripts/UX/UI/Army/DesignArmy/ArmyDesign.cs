using Game.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesign: Singleton<ArmyDesign>
    {
        [SerializeField] private DesignArmyInfo info;
        [SerializeField] private ArmyDesignBoard board;
        [SerializeField] private ArmySearcher searcher;
        [SerializeField] private DesignQuit quitter;
        
        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (quitter.gameObject.activeSelf)
            {
                quitter.gameObject.SetActive(false);
            }
            else
            {
                quitter.gameObject.SetActive(true);
            }
        }

        public void Load(int size)
        {
            info.Load(size);
            board.Load(size);
        }

        public bool IsAllowed(int rank, int file)
        {
            return true;
        }
    }
}