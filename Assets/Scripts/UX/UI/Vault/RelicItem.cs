using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Vault
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text relicName;
        private string relicType;
        [SerializeField] private Image relicImage;

        public void Load(string type)
        {
            relicType = type;
            var info = AssetManager.Ins.RelicData[type];
            relicName.text = Localizer.GetText("relic_name", info.key, null);
            var relicIcon = info.icon;
            
            if (relicIcon == null) return;
            Sprite relicSprite = Sprite.Create(relicIcon, new Rect(0, 0, relicIcon.width, relicIcon.height), Vector2.zero);
            relicImage.sprite = relicSprite;
        }
    }
}