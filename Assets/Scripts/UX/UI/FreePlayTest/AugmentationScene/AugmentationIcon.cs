using System;
using Game.Augmentation;
using Game.Managers;
using Game.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.Tooltip;

namespace UX.UI.FreePlayTest.AugmentationScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AugmentationIcon: MonoBehaviour, IPointerClickHandler
            // IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public AugmentationSlot slot;
        [SerializeField] private Texture2D defaultImage;
        private Sprite defaultImageSprite;
        [SerializeField] private Image image;
        [SerializeField] private TooltipTrigger trigger;
        [NonSerialized] public Transform Parent;
        private Transform oldParent;
        [NonSerialized] public bool Placed;
        [NonSerialized] public int Rank = -1;
        [NonSerialized] public int File = -1;
        public AugmentationInfo Aug;
        public enum Position
        {
            Equipped,
            Unequipped,
        }
        public bool slotInSearchBox = true;
        public Position position = Position.Unequipped;
        void Awake()
        {
            defaultImageSprite = Sprite.Create(
                    defaultImage,
                    new Rect(0, 0, defaultImage.width, defaultImage.height),
                    new Vector2(0.5f, 0.5f) // pivot at center
                );
        }
        public void Load(AugmentationName name)
        {
            
            if (name == AugmentationName.None) // Called when users click at AugmentationIcon in left panel
            {
                position = Position.Unequipped;
                image.sprite = defaultImageSprite;
                Aug = null;
            }
            else //Called when our filter display result or equip an augmentation to Troop
            {
                Aug = AssetManager.Ins.AugmentationData[name];
                position = Position.Equipped;
                gameObject.SetActive(true);
                var sprite = Sprite.Create(
                    Aug.Icon,
                    new Rect(0, 0, Aug.Icon.width, Aug.Icon.height),
                    new Vector2(0.5f, 0.5f) // pivot at center
                );
                image.sprite = sprite;
                SetTooltip();
            }

        }

        private void SetTooltip()
        {
            var pieceName = "Beautiful name";
            var pieceDescriptions = "This is a description";

            trigger.SetText(pieceName, "", pieceDescriptions);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;

            if (slotInSearchBox)
            {
                foreach(var icon in AugmentationManagerUI.Ins.icons)
                {
                    if (icon.slot == slot)
                    {
                        AugmentationManagerUI.Ins.AddAugmentation(Aug);
                        icon.Load(Aug.Name);
                        break;
                    }
                }
            }
            else if (position == Position.Equipped)
            {
                
                // Debug.Log((AugmentationManagerUI.Ins == null) ? "Null augmentationui" : "not null");
                AugmentationManagerUI.Ins.RemoveAugmentation(Aug.Slot);
                Load(AugmentationName.None);

            }
            else if (position == Position.Unequipped)
            {
                // AugmentationFilter.Ins.ToggleFilter(slot);
            }
            

        }
    }
}