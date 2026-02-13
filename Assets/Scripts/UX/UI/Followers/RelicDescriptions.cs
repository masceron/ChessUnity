using Game.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UX.UI.Followers
{
    public class RelicDescriptions : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text description;

        public void Display(RelicInfo info)
        {
            gameObject.SetActive(true);
            nameText.text = Localizer.GetText("relic_name", info.key, null);
            description.text = Localizer.GetText("relic_description", info.key + "_description", null);
        }

        public void Undisplay()
        {
            gameObject.SetActive(false);
        }
    }
}