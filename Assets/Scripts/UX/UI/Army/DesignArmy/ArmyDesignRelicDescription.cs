using Game.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UX.UI.Army.DesignArmy
{
    public class ArmyDesignRelicDescription: MonoBehaviour
    {
        [SerializeField] private RectTransform list;
        [SerializeField] public TMP_Text nameText;
        [SerializeField] private TMP_Text description;

        private void OnDisable()
        {
            list.sizeDelta = ((RectTransform)transform.parent.transform).rect.size;
        }

        private void OnEnable()
        {
            list.sizeDelta = ((RectTransform)transform.parent.transform).sizeDelta -
                             ((RectTransform)transform).sizeDelta;
        }

        public void Display(RelicInfo relic)
        {
            gameObject.SetActive(true);
            nameText.text = Localizer.GetText("relic_name", relic.key, null);
            description.text = Localizer.GetText("relic_description", relic.key + "_description", null);
        }

        public void Undisplay()
        {
            gameObject.SetActive(false);
        }
    }
}