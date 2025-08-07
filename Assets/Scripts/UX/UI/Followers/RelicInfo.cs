using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicInfo: MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [SerializeField] private TMP_Text description;
        
        public void Undisplay()
        {
            gameObject.SetActive(false);
        }
    }
}