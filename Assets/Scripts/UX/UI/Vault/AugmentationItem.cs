using Game.Augmentation;
using Game.Common;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.Tooltip;

namespace UX.UI.Vault
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AugmentationItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text augmentationNameText;
        private AugmentationName augmentationName;
        [SerializeField] private Image augmentationImage;
        private string description;

        public void Load(string name)
        {
            AugmentationHelper.TryStringToAugmentationName(name, out augmentationName);
            var info = AssetManager.Ins.AugmentationData[augmentationName];
            augmentationNameText.text = Localizer.GetText("augmentation_name", info.Key, null);
            description = Localizer.GetText("augmentation_desc", info.Key, null);

            var icon = info.Icon;
            
            if (icon == null) return;
            Sprite sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), Vector2.zero);
            augmentationImage.sprite = sprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Ins.Show(augmentationNameText.text, "", description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Ins.Hide();
        }
    }
}
