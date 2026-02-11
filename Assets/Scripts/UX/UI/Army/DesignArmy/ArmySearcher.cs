using System.Collections.Generic;
using Game.Common;
using Game.Piece;
using Game.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
using ZLinq;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmySearcher: Singleton<ArmySearcher> 
    {
        [SerializeField] protected TMP_InputField searchBar;
        [SerializeField] public Transform list;
        [SerializeField] public GameObject troopDisplay;
        [SerializeField] protected UDictionary<PieceRank, Toggle> filterButtons;

        protected Dictionary<string, PieceInfo> data;
        protected List<PieceInfo> lastSearchResult;
        protected readonly List<PieceInfo> greyOutPieces = new();
        protected string lastKeyword;
        public readonly List<ArmyDesignTroop> Pool = new();
        
        // protected void Awake()
        // {
        //     Data = AssetManager.Ins.PieceData;
        //     ArmyDesignBoard.Ins.OnAddTroop += (t) => FilterByCondition();
        //     ArmyDesignBoard.Ins.OnRemoveTroop += (t) => FilterByCondition();
        //     FilterByCondition();
        //     Load();
        // }
        public virtual void Load()
        {
            data = AssetManager.Ins.PieceData;
            ArmyDesign.Ins.board.OnAddTroop += _ => FilterByCondition();
            ArmyDesign.Ins.board.OnRemoveTroop += _ => FilterByCondition();
            lastSearchResult = data.Values.ToList();
            FilterByCondition();
            SearchByKeyword("");
        }

        protected readonly SortedSet<PieceRank> pieceFilters = new()
        {
            PieceRank.Commander,
            PieceRank.Champion,
            PieceRank.Elite,
            PieceRank.Common,
            PieceRank.Swarm,
            PieceRank.Summoned,
            PieceRank.Construct
        };
        protected void ToggleFilter(PieceRank pieceRank){
            ToggleFilter((int)pieceRank);
        }
        //Bật tắt một filter, ví dụ lọc hoặc không lọc Commander
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

        protected void AddToFilter(PieceRank toFilter)
        {
            pieceFilters.Add(toFilter);
            
            var result = data.Values.Where(p => 
                pieceFilters.Contains(p.rank) && 
                Localizer.GetText("piece_name", p.key, null).ToLower().Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult))
            {
                lastSearchResult = result;
            }

            DisplaySearchResult();
        }

        protected void RemoveFromFilter(PieceRank toRemove)
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
                lastSearchResult = data.Values.Where(p =>
                    pieceFilters.Contains(p.rank) &&
                    Localizer.GetText("piece_name", p.key, null).ToLower().Contains(start)).ToList();
            }

            lastKeyword = start;
            DisplaySearchResult();
        }

        protected void DisplaySearchResult()
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

                // if (Pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                // {
                Pool[i].Load(lastSearchResult[i], greyOutPieces.Contains(lastSearchResult[i]));
                // }
            }
        }
        // Dựa trên một vài luật mà game đưa ra, một số Piece có thể sẽ không còn đặt được, bị greyout sau đó
        protected void FilterByCondition(){
            greyOutPieces.Clear();
            // Lọc theo một số condition 
            Dictionary<PieceInfo, int> counts = new();
            
            foreach (var pieceInfo in ArmyDesign.Ins.board.Troops.Select(tr => AssetManager.Ins.PieceData[tr.PieceType]))
            {
                counts.TryAdd(pieceInfo, 0);
                counts[pieceInfo]++;
                //Chỉ có một Commander ở mỗi side, nên khi add vào thì phải grey out toàn bộ commander còn lại
                if (pieceInfo.rank == PieceRank.Commander)
                {
                    foreach (var t in AssetManager.Ins.PieceData.Values)
                    {
                        if (t.rank == PieceRank.Commander)
                        {
                            greyOutPieces.Add(t);
                        }
                    }
                }
                // Champion: Tối đa 2 quân giống nhau / biến thể của nhau 
                if (pieceInfo.rank == PieceRank.Champion && counts[pieceInfo] == 2)
                {
                    greyOutPieces.Add(pieceInfo);
                }
                //Elite: Tối đa 4 quân giống nhau / biến thể của nha
                if (pieceInfo.rank == PieceRank.Elite && counts[pieceInfo] == 4)
                {
                    greyOutPieces.Add(pieceInfo);
                }
                //Common: tối đa 10 quân giống nhau / biến thể của nhau
                if (pieceInfo.rank == PieceRank.Common && counts[pieceInfo] == 10)
                {
                    greyOutPieces.Add(pieceInfo);
                }
                // Construct: 1 quân mỗi bên, giới hạn ở nửa bàn cờ bên mình
                if (pieceInfo.rank == PieceRank.Construct)
                {
                    foreach (var t in ArmyDesign.Ins.board.Troops)
                    {
                        if (t.GetPieceInfo().rank == PieceRank.Construct)
                        {
                            greyOutPieces.Add(t.GetPieceInfo());
                        }
                    }
                }
            }
            DisplaySearchResult();
        }
    }
}