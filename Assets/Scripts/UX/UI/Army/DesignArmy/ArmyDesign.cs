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
        private int size;
        private Game.Save.Army.Army army;
        
        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            quitter.gameObject.SetActive(!quitter.gameObject.activeSelf);
        }

        public void Load(int s)
        {
            size = s;
            army.BoardSize = (ushort) s;
            info.Load(size);
            board.Load(size);
        }

        private void LoadSave(Game.Save.Army.Army saved)
        {
            army = saved;
        }

        public void Save()
        {
            SetTroops();
        }

        private void SetTroops()
        {
            board.troops.Sort();
            army.Troops = board.troops.ToArray();
            foreach (var troop in army.Troops)
            {
                Debug.Log($"{troop.Type} {troop.Rank} {troop.File}");
            }
        }
    }
}