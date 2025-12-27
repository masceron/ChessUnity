using UnityEngine;
using UX.UI;

namespace UX.UI.MurkyTower
{
    public class MurkyTowerUI : MonoBehaviour
    {
        public void OnClickReturn()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}