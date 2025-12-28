using UnityEngine;

namespace UX.UI.Vault
{
    public class Vault : MonoBehaviour
    {
        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}