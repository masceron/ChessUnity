using System;
using Game.ScriptableObjects;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.Trader
{
    public class TraderItemUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI Components")] [SerializeField]
        private Image itemIcon;

        [SerializeField] private TextMeshProUGUI itemNameText;
        private bool _isSellMode;

        private object _itemData;
        private TraderItemUIType _type;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked?.Invoke(_itemData, _isSellMode);
            Debug.Log("Item clicked");
        }

        public event Action<object, bool> OnItemClicked;

        public void Setup(object data, TraderItemUIType type, bool isSell)
        {
            _itemData = data;
            _isSellMode = isSell;
            _type = type;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_itemData == null) return;

            var key = string.Empty;
            var table = string.Empty;
            Texture2D icon = null;

            foreach (Transform child in itemIcon.transform) Destroy(child.gameObject);
            itemIcon.enabled = true;

            switch (_type)
            {
                case TraderItemUIType.Relic:
                    if (_itemData is RelicInfo relic)
                    {
                        key = relic.key;
                        table = "relic_name";
                        icon = relic.icon;
                    }

                    break;
                case TraderItemUIType.Creature:
                    if (_itemData is PieceInfo piece)
                    {
                        key = piece.key;
                        table = "piece_name";
                        if (piece.prefab != null)
                        {
                            var iconObj = new GameObject("Icon3D");
                            iconObj.transform.SetParent(itemIcon.transform, false);
                            var rect = iconObj.AddComponent<RectTransform>();
                            rect.anchorMin = Vector2.zero;
                            rect.anchorMax = Vector2.one;
                            rect.offsetMin = rect.offsetMax = Vector2.zero;
                            var uiObject3D = iconObj.AddComponent<UIObject3D>();
                            uiObject3D.ObjectPrefab = piece.prefab.transform;
                            itemIcon.enabled = false;
                        }
                    }

                    break;
                case TraderItemUIType.Augmentation:
                    if (_itemData is AugmentationInfo augmentation)
                    {
                        key = augmentation.Key;
                        table = "augmentation_name";
                        icon = augmentation.Icon;
                    }

                    break;
            }

            if (!string.IsNullOrEmpty(key)) itemNameText.text = Localizer.GetText(table, key, null);
            if (icon != null)
                itemIcon.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), Vector2.one * 0.5f);
            else
                itemIcon.sprite = null;
            itemNameText.enabled = true;
        }
    }

    public enum TraderItemUIType
    {
        Relic,
        Creature,
        Augmentation
    }
}