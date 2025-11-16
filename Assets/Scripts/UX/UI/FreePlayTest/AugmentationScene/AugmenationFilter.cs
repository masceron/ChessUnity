
using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;
using Game.Managers;
using Game.Augmentation;

namespace UX.UI.FreePlayTest
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AugmentationFilter: Singleton<AugmentationFilter>
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] public Transform list;
        [SerializeField] public GameObject augDisplay;
        [SerializeField] private UDictionary<AugmentationSlot, AugCategoryButton> filterButtons;

        private Dictionary<AugmentationName, AugmentationInfo> Data;
        private List<AugmentationInfo> lastSearchResult;
        private string lastKeyword;
        public readonly List<AugmentationIcon> Pool = new();
        
        private void Awake()
        {
            base.Awake();
            Data = AssetManager.Ins.AugmentationData;
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
            lastSearchResult = Data.Values.Where(p => p.Slot == filterEnum).ToList();
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

            lastSearchResult = Data.Values.ToList();
            lastKeyword = start;
            DisplaySearchResult();
        }

        private void DisplaySearchResult()
        {
            var needed = lastSearchResult.Count - Pool.Count;
            switch (needed)
            {
                case > 0:
                    {
                        for (var i = 1; i <= needed; i++)
                        {
                            Pool.Add(Instantiate(augDisplay, list).GetComponent<AugmentationIcon>());
                        }

                        break;
                    }
                case < 0:
                    {
                        for (var i = Pool.Count - 1; i > lastSearchResult.Count - 1; i--)
                        {
                            Pool[i].gameObject.SetActive(false);
                        }

                        break;
                    }
            }

            for (var i = 0; i < lastSearchResult.Count; i++)
            {
                var obj = Pool[i].gameObject;
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }

                // if (Pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                // {
                Pool[i].Load(lastSearchResult[i].Name);
                // }
            }
        }
        
    }
}