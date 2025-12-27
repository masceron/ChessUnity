using UnityEngine;

namespace UX.UI.Training_Ground
{
    public class TrainingGroundUI : MonoBehaviour
    {
        public void OnClickReturn()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}