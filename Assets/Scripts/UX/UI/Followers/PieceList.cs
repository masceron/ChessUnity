using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Data.Pieces;
using Game.Piece;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    public class PieceList: MonoBehaviour
    {
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] private Transform list;
        [SerializeField] private GameObject pieceDisplay;
        [SerializeField] private UDictionary<PieceRank, Toggle> filterButtons;

        private Dictionary<PieceType, PieceObject> piecesData;
        private List<PieceObject> lastSearchResult;
        private string lastKeyword;
        private readonly List<PieceLogo> pool = new();
        
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

        public void Load(Dictionary<PieceType, PieceObject> p)
        {
            piecesData = p;
            SearchByKeyword("");
        }

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
            
            var result = piecesData.Values.Where(p => 
                pieceFilters.Contains(p.rank) && 
                p.pieceName.Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult))
            {
                lastSearchResult = result;
            }

            Display();
        }

        private void RemoveFromFilter(PieceRank toRemove)
        {
            pieceFilters.Remove(toRemove);
            
            var result = lastSearchResult.Where(p => 
                pieceFilters.Contains(p.rank) && 
                p.pieceName.Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult))
            {
                lastSearchResult = result;
            }

            Display();
        }
        
        public void SearchByKeyword(string start)
        {
            start = start.ToLower();
            
            if (lastKeyword != null && start.StartsWith(lastKeyword))
            {
                var result = lastSearchResult.Where(p => 
                    pieceFilters.Contains(p.rank) && 
                    p.pieceName.Contains(start)).ToList();

                if (result.SequenceEqual(lastSearchResult)) return;
                lastSearchResult = result;
            }
            else
            {
                lastSearchResult = piecesData.Values.Where(p => 
                    pieceFilters.Contains(p.rank) &&  
                    p.pieceName.Contains(start)).ToList();
            }
            
            lastKeyword = start;
            Display();
        }
        
        private void Display()
        {
            var needed = lastSearchResult.Count - pool.Count;
            switch (needed)
            {
                case > 0:
                {
                    for (var i = 1; i <= needed; i++)
                    {
                        pool.Add(Instantiate(pieceDisplay, list).GetComponent<PieceLogo>());
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

                if (pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                {
                    pool[i].Load(lastSearchResult[i]);
                }
            }
        }
    }

    
}