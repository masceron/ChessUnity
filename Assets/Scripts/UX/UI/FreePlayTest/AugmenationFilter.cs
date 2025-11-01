


using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Piece;
using Game.ScriptableObjects;
using Game.Save.Army;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Managers;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AugmentationFilter: MonoBehaviour
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] public Transform list;
        [SerializeField] public GameObject troopDisplay;
        [SerializeField] private UDictionary<PieceRank, Toggle> filterButtons;

        public Dictionary<PieceType, PieceInfo> Data;
        private List<PieceInfo> lastSearchResult;
        private List<PieceInfo> greyOutPieces = new();
        private string lastKeyword;
        public readonly List<ArmyDesignTroop> Pool = new();
        
        private void Awake()
        {
            Data = piecesData.piecesData.Dictionary;
            ArmyDesignBoard.Ins.OnAddTroop += (t) => FilterByCondition();
            ArmyDesignBoard.Ins.OnRemoveTroop += (t) => FilterByCondition();
            Load();
        }
        private void CheckConditionAfterAdd(Troop troop)
        {
        //     PieceInfo pieceInfo = AssetManager.Ins.PieceData[troop.Type];
        //     if (pieceInfo.rank == PieceRank.Commander)
        //     {
        //         foreach(var info in AssetManager.Ins.PieceData){
        //             if (info.Value.type == PieceRank.Commander){
                        
        //             }
        //         }
        //     }
        //     else if (pieceInfo.rank == PieceRank.Champion){
        //         if ()
        //     }

        }
        private void CheckConditionAfterRemoval(Troop troop){

        }
        private void Load()
        {
            // không có filter nghĩa là lấy hết toàn bộ các Piece hiện đang sở hữu
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
        private void ToggleFilter(PieceRank pieceRank){
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

                // if (Pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                // {
                Pool[i].Load(lastSearchResult[i], greyOutPieces.Contains(lastSearchResult[i]));
                // }
            }
        }
        // Dựa trên một vài luật mà game đưa ra, một số Piece có thể sẽ không còn đặt được, bị greyout sau đó
        private void FilterByCondition(){
            greyOutPieces.Clear();
            // Lọc theo một số condition 
            Dictionary<PieceInfo, int> counts = new();
            foreach (Troop tr in ArmyDesignBoard.Ins.Troops)
            {
                if (tr.Side != ArmyDesign.Ins.choosenSide)
                {
                    continue;
                }
                PieceInfo pieceInfo = AssetManager.Ins.PieceData[tr.Type];

                if (!counts.ContainsKey(pieceInfo))
                    counts[pieceInfo] = 0;
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
                    foreach (Troop t in ArmyDesignBoard.Ins.Troops)
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