using System;
using Game.Common;
using Game.Managers;
using Game.Save.Player;
using Game.Save.Relics;
using Game.ScriptableObjects;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Trader
{
    public class TraderConfirmPopup : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject content;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI confirmButtonText;
        [SerializeField] private Button closeButton;

        private object _itemData;
        private bool _isSellMode;

        public event Action OnTransactionCompleted;

        private void Awake()
        {
            if (confirmButton) confirmButton.onClick.AddListener(OnConfirmClick);
            if (closeButton) closeButton.onClick.AddListener(Hide);
        }

        public void Show(object data, bool isSell)
        {
            _itemData = data;
            _isSellMode = isSell;
            if (content) content.SetActive(true);
            UpdateUI();
        }

        public void Hide()
        {
            if (content) content.SetActive(false);
            _itemData = null;
        }

        private void UpdateUI()
        {
            if (_itemData == null) return;

            string key = string.Empty;
            string table = string.Empty;
            string descTable = string.Empty;
            Texture2D icon = null;

            // Clear previous 3D icons
            foreach (Transform child in itemIcon.transform) Destroy(child.gameObject);
            itemIcon.enabled = true;

            if (_itemData is RelicInfo relic)
            {
                key = relic.key;
                table = "relic_name";
                descTable = "relic_description";
                icon = relic.icon;
            }
            else if (_itemData is PieceInfo piece)
            {
                key = piece.key;
                table = "piece_name";
                descTable = "piece_description";
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
            else if (_itemData is AugmentationInfo augmentation)
            {
                key = augmentation.Key;
                table = "augmentation_name";
                descTable = "augmentation_description";
                icon = augmentation.Icon;
            }

            if (!string.IsNullOrEmpty(key))
            {
                if (itemNameText) itemNameText.text = Localizer.GetText(table, key, null);
                if (itemDescriptionText) itemDescriptionText.text = Localizer.GetText(descTable, key + "_description", null);
            }

            if (icon != null && itemIcon)
            {
                itemIcon.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), Vector2.one * 0.5f);
            }

            if (confirmButtonText) confirmButtonText.text = _isSellMode ? "Sell" : "Buy";
        }

        private void OnConfirmClick()
        {
            if (_itemData == null) return;

            if (_itemData is RelicInfo relic)
            {
                var relics = RelicSaveLoader.GetCollectedRelics();
                if (_isSellMode) { if (relics.Contains(relic.key)) relics.Remove(relic.key); }
                else { if (!relics.Contains(relic.key)) relics.Add(relic.key); }
            }
            else if (_itemData is PieceInfo piece)
            {
                var units = PlayerSaveLoader.Player.CollectedUnits;
                if (_isSellMode) { if (units.Contains(piece.key)) units.Remove(piece.key); }
                else { if (!units.Contains(piece.key)) units.Add(piece.key); }
            }
            else if (_itemData is AugmentationInfo aug)
            {
                var augs = PlayerSaveLoader.Player.CollectedAugmentations;
                if (_isSellMode) { if (augs.Contains(aug.Name.ToString())) augs.Remove(aug.Name.ToString()); }
                else { if (!augs.Contains(aug.Name.ToString())) augs.Add(aug.Name.ToString()); }
            }

            OnTransactionCompleted?.Invoke();
            Hide();
        }
    }
}