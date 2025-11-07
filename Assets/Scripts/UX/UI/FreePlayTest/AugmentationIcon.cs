using System;
using Game.ScriptableObjects;
using UI.UIObject3D.Scripts;
using Game.Augmentation;
using Game.Save.Army;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UX.UI.Army.DesignArmy;
using UX.UI.Tooltip;
using Game.Managers;

namespace UX.UI.FreePlayTest
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
                this.gameObject.SetActive(true);
                Sprite sprite = Sprite.Create(
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

        // public void OnBeginDrag(PointerEventData eventData)
        // {
        //     oldParent = transform.parent;
        //     transform.SetParent(FindAnyObjectByType<Canvas>().transform);
        //     image.raycastTarget = false;
        //     Parent = null;
        //     trigger.enabled = false;
        //     TooltipManager.Ins.Disable();

        //     if (Placed) return;
            
        //     var searcher = FindAnyObjectByType<AugmentationFilter>();
        //     var pool = searcher.Pool;
        //     var idx = pool.IndexOf(this);
        //     var obj = Instantiate(this, searcher.list);
            
        //     obj.transform.SetSiblingIndex(idx);
        //     obj.GetComponent<Image>().raycastTarget = true;
        //     obj.trigger.enabled = true;
            
        //     obj.Load(Aug);
        //     pool[idx] = obj;
        // }

        // public void OnDrag(PointerEventData eventData)
        // {
        //     transform.position = eventData.position;
        // }

        
        // public void OnEndDrag(PointerEventData eventData)
        // {
        //     FindAnyObjectByType<ArmyDesignBoard>().UnSet();
        //     trigger.enabled = true;
        //     TooltipManager.Ins.Enable();
        //     if (!Parent)
        //     {
        //         if (!Placed)
        //         {
        //             Destroy(gameObject);
        //         }
        //         else
        //         {
        //             transform.SetParent(oldParent);
        //             image.raycastTarget = true;
        //         }
        //     }
        //     else
        //     {
        //         transform.SetParent(Parent);

        //         var size = Parent.transform.GetComponent<GridLayoutGroup>().cellSize;
        //         GetComponent<RectTransform>().sizeDelta = size;
                
        //         image.raycastTarget = true;
        //         Placed = true;
        //     }
        // }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;

            if (slotInSearchBox)
            {
                Debug.Log("Click");
                foreach(AugmentationIcon icon in AugmentationManagerUI.Ins.icons)
                {
                    if (icon.slot == this.slot)
                    {
                        AugmentationManagerUI.Ins.AddAugmentation(Aug);
                        icon.Load(this.Aug.Name);
                        break;
                    }
                }
            }
            else if (position == Position.Equipped)
            {
                
                // Debug.Log((AugmentationManagerUI.Ins == null) ? "Null augmentationui" : "not null");
                AugmentationManagerUI.Ins.RemoveAugmentation(this.Aug.Slot);
                Load(AugmentationName.None);

            }
            else if (position == Position.Unequipped)
            {
                AugmentationFilter.Ins.ToggleFilter(slot);
            }
            

        }
    }
}