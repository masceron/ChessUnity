using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Piece;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmySearcher: MonoBehaviour
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] public Transform list;
        [SerializeField] public GameObject troopDisplay;
        [SerializeField] private UDictionary<PieceRank, Toggle> filterButtons;

        public Dictionary<string, PieceInfo> Data;
        private List<PieceInfo> lastSearchResult;
        private string lastKeyword;
        public readonly List<ArmyDesignTroop> Pool = new();
        
        private void Awake()
        {
            Data = new Dictionary<string, PieceInfo>();
            foreach (var pieceInfo in piecesData.piecesData)
            {
                Data.Add(pieceInfo.key, pieceInfo);
            }
            Load();
        }

        private void Load()
        {
            SearchByKeyword("");
        }

        private readonly SortedSet<PieceRank> pieceFilters = new()
        {
            PieceRank.Commander,
            PieceRank.Champion,
            PieceRank.Elite,
            PieceRank.Common,
            PieceRank.Swarm,
            PieceRank.Summoned,
            PieceRank.Construct
        };

        public void ToggleFilter(int filter)
        {
            var filterEnum = (PieceRank)filter;
            if (filterButtons[filterEnum].isOn)
            {
                var color = filterButtons[filterEnum].GetComponent<Image>().color;
                color.a = 1f;
                filterButtons[filterEnum].GetComponent<Image>().color = color;
                
                AddToFilter(filterEnum);
            }
            else
            {
                var color = filterButtons[filterEnum].GetComponent<Image>().color;
                color.a = 0.3f;
                filterButtons[filterEnum].GetComponent<Image>().color = color;
                
                RemoveFromFilter(filterEnum);
            }
            
        }

        private void AddToFilter(PieceRank toFilter)
        {
            pieceFilters.Add(toFilter);
            
            var result = Data.Values.Where(p => 
                pieceFilters.Contains(p.rank) && 
                Localizer.GetText("piece_name", p.key, null).ToLower().Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult))
            {
                lastSearchResult = result;
            }

            DisplaySearchResult();
        }

        private void RemoveFromFilter(PieceRank toRemove)
        {
            pieceFilters.Remove(toRemove);
            
            var result = lastSearchResult.Where(p => 
                pieceFilters.Contains(p.rank) && 
                Localizer.GetText("piece_name", p.key, null).Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult))
            {
                lastSearchResult = result;
            }

            DisplaySearchResult();
        }
        
        public void SearchByKeyword(string start)
        {
            start = start.ToLower();
            
            if (lastKeyword != null && start.StartsWith(lastKeyword))
            {
                var result = lastSearchResult.Where(p => 
                    pieceFilters.Contains(p.rank) && 
                    Localizer.GetText("piece_name", p.key, null).ToLower().Contains(start)).ToList();

                if (result.SequenceEqual(lastSearchResult)) return;
                lastSearchResult = result;
            }
            else
            {
                lastSearchResult = Data.Values.Where(p => 
                    pieceFilters.Contains(p.rank) &&  
                    Localizer.GetText("piece_name", p.key, null).ToLower().Contains(start)).ToList();
            }
            
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
                        Pool.Add(Instantiate(troopDisplay, list).GetComponent<ArmyDesignTroop>());
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

                if (Pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                {
                    Pool[i].Load(lastSearchResult[i]);
                }
            }
        }
    }
}