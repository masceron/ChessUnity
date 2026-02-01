using System.Collections.Generic;
using Game.Save.Player;
using System.Linq;
using Game.Common;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using ZLinq;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicList: Singleton<RelicList>, IPointerClickHandler
    {
        [SerializeField] private RelicsData relicsData;
        [SerializeField] private GameObject relicDisplay;

        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] private Transform list;
        [SerializeField] private RelicDescriptions descriptions;

        private readonly List<RelicLogo> pool = new();
        private Dictionary<string, RelicInfo> data;
        private List<RelicInfo> searchResult;
        private string lastKeyword;
        
        private bool selecting;

        protected void OnEnable()
        {
            data = new Dictionary<string, RelicInfo>();
            
            if (PlayerSaveLoader.Player.CollectedRelics != null)
            {
                var collectedSet = new HashSet<string>(PlayerSaveLoader.Player.CollectedRelics);

                foreach (var relicInfo in relicsData.relicsData)
                {
                    if (collectedSet.Contains(relicInfo.key))
                    {
                        if (!data.ContainsKey(relicInfo.key))
                        {
                            data.Add(relicInfo.key, relicInfo);
                        }
                    }
                }
            }
            
            SearchByKeyword("");
        }

        private void OnDisable()
        {
            Close();
        }

        public void Close()
        {
            selecting = false;
            descriptions.Undisplay();
        }

        public void DisplayInfo(RelicInfo relic)
        {
            if (selecting) return;
            descriptions.Display(relic);
        }

        public void Undisplay()
        {
            if (selecting) return;
            descriptions.Undisplay();
        }
        
        public void Select(RelicInfo relic)
        {
            selecting = false;
            DisplayInfo(relic);
            selecting = true;
        }

        public void SearchByKeyword(string start)
        {
            start = start.ToLower();
            
            if (lastKeyword != null && start.StartsWith(lastKeyword))
            {
                var result = searchResult.Where(r => 
                    r.key.Contains(start)).ToList();

                if (result.SequenceEqual(searchResult)) return;
                searchResult = result;
            }
            else
            {
                searchResult = data.Values.Where(r => 
                    r.key.Contains(start)).ToList();
            }
            
            lastKeyword = start;
            DisplaySearchResult();
        }

        private void DisplaySearchResult()
        {
            var needed = searchResult.Count - pool.Count;
            switch (needed)
            {
                case > 0:
                {
                    for (var i = 1; i <= needed; i++)
                    {
                        pool.Add(Instantiate(relicDisplay, list).GetComponent<RelicLogo>());
                    }

                    break;
                }
                case < 0:
                {
                    for (var i = pool.Count - 1; i > searchResult.Count - 1; i--)
                    {
                        pool[i].gameObject.SetActive(false);
                    }

                    break;
                }
            }
            
            for (var i = 0; i < searchResult.Count; i++)
            {
                var obj = pool[i].gameObject;
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }

                if (pool[i].Relic != searchResult[i])
                {
                    pool[i].Load(searchResult[i]);
                }
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || !selecting) return;
            selecting = false;
            Undisplay();
        }
    }
}