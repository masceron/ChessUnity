using UnityEngine;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicList: MonoBehaviour
    {
        [SerializeField] private RelicInfo relicInfo;

        private void OnDisable()
        {
            Close();
        }

        public void Close()
        {
            relicInfo.Undisplay();
        }

        private void Undisplay()
        {
            relicInfo.Undisplay();
        }
    }
}