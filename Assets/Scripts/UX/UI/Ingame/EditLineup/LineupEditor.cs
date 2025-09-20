using Game.Save.Army;
using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.Ingame.EditLineup
{
    public class LineupEditor: MonoBehaviour
    {
        [SerializeField] private LineupInfo info;
        [SerializeField] private ArmyDesignBoard board;
        [SerializeField] private ArmySearcher searcher;
        [SerializeField] private ArmyRelicSearcher relicSearcher;
        private int size;
        private Game.Save.Army.Army army;
        
        public void Load(int s)
        {
            size = s;
            board.Load(size);
        }

        public void LoadFromSave(string armyName)
        {
            var armyToLoad = ArmySaveLoader.Read(armyName);
            board.LoadSave(armyToLoad.Troops);
            relicSearcher.Load(armyToLoad.Relic);
        }
    }
}