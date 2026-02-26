using UnityEngine;
using UX.UI.Loader;

namespace UX.UI.Assignments
{
    public class AssignmentsUI : MonoBehaviour
    {
        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }

        public void LoadMap1()
        {
            SceneLoader.LoadSceneWithLoadingScreen(4);
        }
    }
}