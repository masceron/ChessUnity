using UnityEngine;
using UX.UI;

namespace UX.UI.Trader
{
    public class TraderUI : MonoBehaviour
    {
        public void OnClickReturn()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}