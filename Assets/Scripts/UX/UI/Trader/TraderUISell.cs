using UnityEngine;
using System.Collections.Generic;
using Game.Save.Relics;
using Game.Save.Player;
using Game.Managers;

namespace UX.UI.Trader
{
    public class TraderUISell : MonoBehaviour
    {
        [Header("Content Containers")]
        [SerializeField] private Transform relicsContent;
        [SerializeField] private Transform creaturesContent;
        [SerializeField] private Transform augmentationsContent;

        [Header("Settings")]
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private TraderConfirmPopup confirmPopup;


        private void OnEnable()
        {
            LoadPlayerInventory();
            confirmPopup.OnTransactionCompleted += OnTransactionCompleted;
            confirmPopup.Hide();
        }

        private void OnDisable()
        {
            confirmPopup.OnTransactionCompleted -= OnTransactionCompleted;
        }

        private void OnTransactionCompleted()
        {
            PlayerSaveLoader.Save();
            LoadPlayerInventory();
        }

        public void LoadPlayerInventory()
        {
            ClearContent(relicsContent);
            ClearContent(creaturesContent);
            ClearContent(augmentationsContent);

            var allRelics = AssetManager.Ins.RelicData;
            var allCreatures = AssetManager.Ins.PieceData;
            var allAugmentations = AssetManager.Ins.AugmentationData;

            var ownedRelics = RelicSaveLoader.GetCollectedRelics();
            var ownedCreatures = PlayerSaveLoader.Player.CollectedUnits;
            var ownedAugmentations = PlayerSaveLoader.Player.CollectedAugmentations;

            // 1. Load Relics
            foreach (var relic in ownedRelics)
            {
                if (allRelics.TryGetValue(relic, out var relicInfo)) 
                    SpawnItem(relicInfo, TraderItemUIType.Relic, relicsContent);
            }

            // 2. Load Creatures
            foreach (var creature in ownedCreatures)
            {
                if (allCreatures.TryGetValue(creature, out var pieceInfo)) 
                    SpawnItem(pieceInfo, TraderItemUIType.Creature, creaturesContent);
            }

            // 3. Load Augmentations
            foreach (var augmentation in ownedAugmentations)
            {
                if (System.Enum.TryParse<Game.Augmentation.AugmentationName>(augmentation, out var augmentationName))
                {
                    if (allAugmentations.TryGetValue(augmentationName, out var augmentationInfo)) 
                        SpawnItem(augmentationInfo, TraderItemUIType.Augmentation, augmentationsContent);
                }
            }
        }

        private void ClearContent(Transform content)
        {
            if (content == null) return;
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }

        private void SpawnItem(object data, TraderItemUIType type, Transform parent)
        {
            if (itemPrefab == null || parent == null) return;
            
            GameObject newItem = Instantiate(itemPrefab, parent);
            var itemUI = newItem.GetComponent<TraderItemUI>();
            itemUI.Setup(data, type, isSell: true);
            itemUI.OnItemClicked += OnItemClicked;
        }

        private void OnItemClicked(object data, bool isSell)
        {
            confirmPopup.Show(data, isSell);
        }

    }
}