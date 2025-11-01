using Game.Save.Army;
using TMPro;
using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.Followers
{
    public class SavedArmy: MonoBehaviour
    {
        private Game.Save.Army.Army army;
        [SerializeField] private TMP_Text armyName;
        [SerializeField] private TMP_Text boardSize;

        public void Load(Game.Save.Army.Army load)
        {
            army = load;
            armyName.text = load.Name;
            boardSize.text = $"{load.BoardSize} x {load.BoardSize}";
        }

        public void Click()
        {
            UIManager.Ins.Load(CanvasID.DesignArmy);
            ArmyDesign.Ins.Load(army.BoardSize, army);
        }
        public void ClickToFreePlayDesignArmy()
        {
            UIManager.Ins.Load(CanvasID.FreePlayDesignArmy);
            ArmyDesign.Ins.Load(army.BoardSize, army);
        }
        public void Delete()
        {
            ArmySaveLoader.Remove(army.Name);
            Destroy(gameObject);
        }
    }
}