using UnityEngine;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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