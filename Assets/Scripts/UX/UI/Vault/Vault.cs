using System.Collections.Generic;
using UnityEngine;

namespace UX.UI.Vault
{
    public class Vault : MonoBehaviour
    {
        [SerializeField] private GameObject relicItem;
        [SerializeField] private GameObject relicContainer;
        
        private readonly List<string> relicsInVault = new()
        {
            "relic_black_pearl",
            "relic_common_pearl",
            "relic_eye_of_mimic",
            "relic_frost_sigil",
            "relic_mangrove_charm",
            "relic_rotting_scythe",
            "relic_seafoam_phial",
            "relic_sirens_harpoon",
            "relic_storm_capacitor"
        };
        
        //private readonly List<string> relicsInVault2 = new()

        private void OnEnable()
        {
            ClearRelicItem();
            LoadRelicItem();

            
        }

        private void LoadRelicItem()
        {
            foreach (var relic in relicsInVault)
            {
                var relicImage = Instantiate(relicItem, relicContainer.transform);
                relicImage.GetComponent<RelicItem>().Load(relic);
            }
        }

        private void ClearRelicItem()
        {
            for (var i = 0; i < relicContainer.transform.childCount; i++)
            {
                Destroy(relicContainer.transform.GetChild(i));
            }
        }

        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas(); 
        }
    }
}