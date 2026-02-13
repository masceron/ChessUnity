using Game.Managers;
using Game.Save.Player;
using Game.Save.Relics;
using UnityEngine;

namespace UX.UI.Trader
{
    public class TraderUIBuy : MonoBehaviour
    {
        [Header("Content Containers")] [SerializeField]
        private Transform relicsContent;

        [SerializeField] private Transform creaturesContent;

        [Header("Settings")] [SerializeField] private GameObject itemPrefab;

        [SerializeField] private TraderConfirmPopup confirmPopup;


        private void OnEnable()
        {
            LoadShopInventory();
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
            LoadShopInventory();
        }


        public void LoadShopInventory()
        {
            ClearContent(relicsContent);
            ClearContent(creaturesContent);

            // TODO: Retrieve data from AssetManager and PlayerSaveLoader

            // 1. Get all available items
            var allRelics = AssetManager.Ins.RelicData;
            var allCreatures = AssetManager.Ins.PieceData;

            // 2. Get owned items for filtering
            var ownedRelics = RelicSaveLoader.GetCollectedRelics();
            var ownedCreatures = PlayerSaveLoader.Player.CollectedUnits;

            // 3. Load Relics (Exclude owned)
            foreach (var relic in allRelics)
                if (!ownedRelics.Contains(relic.Key))
                    SpawnItem(relic.Value, TraderItemUIType.Relic, relicsContent);

            // 4. Load Creatures (Exclude owned)
            foreach (var creature in allCreatures)
                if (!ownedCreatures.Contains(creature.Key))
                    SpawnItem(creature.Value, TraderItemUIType.Creature, creaturesContent);
        }

        private void ClearContent(Transform content)
        {
            if (content == null) return;
            foreach (Transform child in content) Destroy(child.gameObject);
        }

        private void SpawnItem(object data, TraderItemUIType type, Transform parent)
        {
            if (itemPrefab == null || parent == null) return;

            var newItem = Instantiate(itemPrefab, parent);
            // TODO: Setup item UI
            var itemUI = newItem.GetComponent<TraderItemUI>();
            itemUI.Setup(data, type, false);
            itemUI.OnItemClicked += OnItemClicked;
        }

        private void OnItemClicked(object data, bool isSell)
        {
            confirmPopup.Show(data, isSell);
        }
    }
}