using UnityEngine;

namespace UX.UI.Followers
{
    public class RelicList: MonoBehaviour
    {
        [SerializeField] private RelicInfo relicInfo;

        private void OnDisable()
        {
            relicInfo.Undisplay();
        }

        public void Undisplay()
        {
            relicInfo.Undisplay();
        }
    }
}