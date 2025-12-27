using UnityEngine;

namespace UX.UI.Vault
{
    public class Vault : MonoBehaviour
    {
        public void OnClickReturn()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}