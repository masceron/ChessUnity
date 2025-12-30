using UnityEngine;

namespace UX.UI.OutworldInvaders
{
    public class OutworldInvadersUI : MonoBehaviour
    {
        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}