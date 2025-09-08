using UnityEngine;
using UX.UI.Army.DesignArmy;

namespace UX.UI.Ingame.EditLineup
{
    public class LineupEditor: MonoBehaviour
    {
        [SerializeField] private LineupInfo info;
        [SerializeField] private ArmyDesignBoard board;
        [SerializeField] private ArmySearcher searcher;
        private int size;
        
        public void Load(int s)
        {
            size = s;
            info.Load(size);
            board.Load(size);
        }
    }
}