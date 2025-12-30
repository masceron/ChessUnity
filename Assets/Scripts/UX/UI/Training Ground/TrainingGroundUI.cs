using UnityEngine;

namespace UX.UI.Training_Ground
{
    public class TrainingGroundUI : MonoBehaviour
    {
        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}