using UnityEngine;

namespace UX.UI.OutworldInvaders
{
    public class OutworldInvadersUI : MonoBehaviour
    {
        public void OnClickReturn()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}