using UnityEngine;

namespace UX.UI.Assignments
{
    public class AssignmentsUI : MonoBehaviour
    {
        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}