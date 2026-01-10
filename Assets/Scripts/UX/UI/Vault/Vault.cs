using System.Collections.Generic;
using Game.Save.Relics;
using UnityEngine;
using Game.Save.Augmentation;

namespace UX.UI.Vault
{
    public class Vault : MonoBehaviour
    {
        [SerializeField] private GameObject relicItem;
        [SerializeField] private GameObject relicContainer;
        [SerializeField] private GameObject augmentationItem;
        [SerializeField] private GameObject augmentationContainer;
        
        private readonly List<string> relicsInVault = new List<string>();
        private readonly List<string> augmentationsInVault = new List<string>();

        
        //private readonly List<string> relicsInVault2 = new()

        private void OnEnable()
        {
            ClearRelicItem();
            LoadRelicItem();
            ClearAugmentationItem();
            LoadAugmentationItem();
        }

        private void LoadRelicItem()
        {
            relicsInVault.Clear();
            relicsInVault.AddRange(RelicSaveLoader.GetCollectedRelics());

            foreach (var relic in relicsInVault)
            {
                var relicImage = Instantiate(relicItem, relicContainer.transform);
                relicImage.GetComponent<RelicItem>().Load(relic);
            }
        }

        private void LoadAugmentationItem()
        {
            augmentationsInVault.Clear();
            augmentationsInVault.AddRange(AugmentationSaveLoader.GetCollectedAugmentations());

            foreach (var aug in augmentationsInVault)
            {
                var item = Instantiate(augmentationItem, augmentationContainer.transform);
                item.GetComponent<AugmentationItem>().Load(aug);
            }
        }

        private void ClearRelicItem()
        {
            for (var i = 0; i < relicContainer.transform.childCount; i++)
            {
                Destroy(relicContainer.transform.GetChild(i).gameObject);
            }
            relicsInVault.Clear();
        }

        private void ClearAugmentationItem()
        {
            for (var i = 0; i < augmentationContainer.transform.childCount; i++)
            {
                Destroy(augmentationContainer.transform.GetChild(i).gameObject);
            }

            augmentationsInVault.Clear();
        }

        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas(); 
        }
    }
}