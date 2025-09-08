using TMPro;
using UnityEngine;

namespace UX.UI.Ingame.EditLineup
{
    public class LineupInfo: MonoBehaviour
    {
        [SerializeField] private TMP_Text boardSize;
        
        public void Load(int size)
        {
            boardSize.text = $"{size} x {size}";
        }
    }
}