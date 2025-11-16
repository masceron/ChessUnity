using System.Collections.Generic;
using System.Linq;
using Game.Augmentation;
using Game.Common;
using Game.Managers;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;

namespace UX.UI.FreePlayTest.AugmentationScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AugmentationFilter: Singleton<AugmentationFilter>
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] public Transform list;
        [SerializeField] public GameObject augDisplay;
        [SerializeField] private UDictionary<AugmentationSlot, AugCategoryButton> filterButtons;

        private Dictionary<AugmentationName, AugmentationInfo> data;
        private List<AugmentationInfo> lastSearchResult;
        private readonly List<AugmentationIcon> pool = new();

        protected override void Awake()
        {
            data = AssetManager.Ins.AugmentationData;
            // không có filter nghĩa là lấy hết toàn bộ các Piece hiện đang sở hữu
            ToggleFilter(0);
        }

        private readonly SortedSet<AugmentationSlot> pieceFilters = new()
        {
                    AugmentationSlot.Optic,
                    AugmentationSlot.Neural,
                    AugmentationSlot.Blood,
                    AugmentationSlot.Fin,
                    AugmentationSlot.Chassis
        };
        public void ToggleFilter(AugmentationSlot slot){
            ToggleFilter((int)slot);
        }
        //Bật tắt một filter, ví dụ lọc hoặc không lọc Commander
        public void ToggleFilter(int filter)
        {
            var filterEnum = (AugmentationSlot)filter;
            foreach(var button in filterButtons)
            {
                button.Value.GreyOut();
            }
            filterButtons[filterEnum].GreyOut();
            lastSearchResult = data.Values.Where(p => p.Slot == filterEnum).ToList();
            DisplaySearchResult();
            
        }

        public void SearchByKeyword(string start)
        {
            start = start.ToLower();

            // if (lastKeyword != null && start.StartsWith(lastKeyword))
            // {
            //     var result = lastSearchResult.Where(p =>
            //         pieceFilters.Contains(p.Slot) &&
            //         Localizer.GetText("piece_name", p.key, null).ToLower().Contains(start)).ToList();

            //     if (result.SequenceEqual(lastSearchResult)) return;
            //     lastSearchResult = result;
            // }
            // else
            // {
            //     lastSearchResult = Data.Values.Where(p =>
            //         pieceFilters.Contains(p.rank) &&
            //         Localizer.GetText("piece_name", p.key, null).ToLower().Contains(start)).ToList();
            // }

            lastSearchResult = data.Values.ToList();
            DisplaySearchResult();
        }

        private void DisplaySearchResult()
        {
            var needed = lastSearchResult.Count - pool.Count;
            switch (needed)
            {
                case > 0:
                    {
                        for (var i = 1; i <= needed; i++)
                        {
                            pool.Add(Instantiate(augDisplay, list).GetComponent<AugmentationIcon>());
                        }

                        break;
                    }
                case < 0:
                    {
                        for (var i = pool.Count - 1; i > lastSearchResult.Count - 1; i--)
                        {
                            pool[i].gameObject.SetActive(false);
                        }

                        break;
                    }
            }

            for (var i = 0; i < lastSearchResult.Count; i++)
            {
                var obj = pool[i].gameObject;
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }

                // if (Pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                // {
                pool[i].Load(lastSearchResult[i].Name);
                // }
            }
        }
        
    }
}