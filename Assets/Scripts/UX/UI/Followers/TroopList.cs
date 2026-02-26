using System.Collections.Generic;
using Game.Common;
using Game.Piece;
using Game.Save.Player;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZLinq;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TroopList : Singleton<TroopList>, IPointerClickHandler
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] private Transform list;
        [SerializeField] private GameObject troopDisplay;
        [SerializeField] private UDictionary<PieceRank, Toggle> filterButtons;
        [SerializeField] private TroopDescriptions troopDescriptions;

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

        private readonly List<TroopLogo> pool = new();

        private Dictionary<string, PieceInfo> data;
        private string lastKeyword;
        private List<PieceInfo> lastSearchResult;
        private bool selecting;

/*        protected override void Awake()
        {
            data = new Dictionary<string, PieceInfo>();
            foreach (var pieceInfo in piecesData.piecesData)
            {
                data.Add(pieceInfo.key, pieceInfo);
            }
            SearchByKeyword("");
        }*/

        private void OnEnable()
        {
            data = new Dictionary<string, PieceInfo>();

            if (PlayerSaveLoader.Player.CollectedUnits == null) return;

            var collectedSet = new HashSet<string>(PlayerSaveLoader.Player.CollectedUnits);

            foreach (var pieceInfo in piecesData.piecesData)
                if (collectedSet.Contains(pieceInfo.key))
                    if (!data.ContainsKey(pieceInfo.key))
                        data.Add(pieceInfo.key, pieceInfo);

            lastKeyword = null;
            searchBar.text = "";
            SearchByKeyword("");
        }

        private void OnDisable()
        {
            Close();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || !selecting) return;
            selecting = false;
            Undisplay();
        }

        public void Close()
        {
            selecting = false;
            troopDescriptions.Undisplay();
        }

        public void Select(string type)
        {
            selecting = false;
            DisplayInfo(type);
            selecting = true;
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

            var result = data.Values.Where(p =>
                pieceFilters.Contains(p.rank) &&
                Localizer.GetText("piece_name", p.key, null).ToLower().Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult)) lastSearchResult = result;

            DisplaySearchResult();
        }

        private void RemoveFromFilter(PieceRank toRemove)
        {
            pieceFilters.Remove(toRemove);

            var result = lastSearchResult.Where(p =>
                pieceFilters.Contains(p.rank) &&
                Localizer.GetText("piece_name", p.key, null).ToLower().Contains(lastKeyword)).ToList();

            if (!result.SequenceEqual(lastSearchResult)) lastSearchResult = result;

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

        private void DisplaySearchResult()
        {
            var needed = lastSearchResult.Count - pool.Count;
            switch (needed)
            {
                case > 0:
                {
                    for (var i = 1; i <= needed; i++)
                        pool.Add(Instantiate(troopDisplay, list).GetComponent<TroopLogo>());

                    break;
                }
                case < 0:
                {
                    for (var i = pool.Count - 1; i > lastSearchResult.Count - 1; i--)
                        pool[i].gameObject.SetActive(false);

                    break;
                }
            }

            for (var i = 0; i < lastSearchResult.Count; i++)
            {
                var obj = pool[i].gameObject;
                if (!obj.activeSelf) obj.SetActive(true);

                if (pool[i].model.ObjectPrefab != lastSearchResult[i].prefab.transform)
                    pool[i].Load(lastSearchResult[i]);
            }
        }

        public void DisplayInfo(string type)
        {
            if (selecting) return;

            troopDescriptions.Display(data[type]);
        }

        public void Undisplay()
        {
            if (selecting) return;
            troopDescriptions.Undisplay();
        }
    }
}