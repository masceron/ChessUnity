using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.Tooltip;

namespace UX.UI.Vault
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text relicName;
        [SerializeField] private Image relicImage;
        private string description;
        private string relicType;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Ins.Show(relicName.text, "", description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Ins.Hide();
        }

        public void Load(string type)
        {
            relicType = type;
            var info = AssetManager.Ins.RelicData[type];
            relicName.text = Localizer.GetText("relic_name", info.key, null);
            description = Localizer.GetText("relic_desc", info.key, null);

            var relicIcon = info.icon;

            if (relicIcon == null) return;
            var relicSprite = Sprite.Create(relicIcon, new Rect(0, 0, relicIcon.width, relicIcon.height), Vector2.zero);
            relicImage.sprite = relicSprite;
        }
    }
}