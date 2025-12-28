using UnityEngine;
using UX.UI;

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