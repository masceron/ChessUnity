using System.Collections.Generic;
using System.Linq;
using System;
using Game.Common;
using Game.Relics;
using Game.Save.Relics;
using Game.ScriptableObjects;
using Game.ScriptableObjects.Collections;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Army.DesignArmy
{
    public class ArmyRelicSearcher: Singleton<ArmyRelicSearcher>, IPointerClickHandler
    {
        [SerializeField] public RectTransform container;
        [SerializeField] private RelicsData relicsData;
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] private RectTransform mainPanel;
        [SerializeField] public Transform list;
        [SerializeField] private ArmyDesignRelicDescription description;
        [SerializeField] public ArmyDesignRelic relicDisplay;
        [SerializeField] private TMP_Text relicText;

        private List<RelicInfo> searchResult;
        private string lastKeyword;
        private readonly List<ArmyDesignRelic> pool = new();
        private RelicType? selecting;
        public Action<RelicType?> OnRelicSelecting;
        private void Awake()
        {
            SearchByKeyword("");
        }

        public void Toggle()
        {
            container.gameObject.SetActive(!container.gameObject.activeSelf);
        }

        private void OnEnable()
        {
            mainPanel.rotation = new Quaternion
            {
                eulerAngles = new Vector3(90f, 0f, 0f)
            };
            Tween.Rotation(mainPanel, new Quaternion
            {
                eulerAngles = Vector3.zero
            }, 0.2f);
        }

        public void Load(Relic? relic)
        {
            relicText.text = !relic.HasValue ? Localizer.GetText("game", "relic", null) 
                : Localizer.GetText("relic_name", relicsData.relicsData[relic.Value.Type].key, null);
        }

        private void OnDisable()
        {
            selecting = null;
            description.Undisplay();
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
                searchResult = relicsData.relicsData.Values.Where(r => 
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
                        pool.Add(Instantiate(relicDisplay.gameObject, list).GetComponent<ArmyDesignRelic>());
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

        public void Select(ArmyDesignRelic relic)
        {
            if (selecting != null)
            {
                if (selecting == relic.Relic.type) return;
            }
            selecting = relic.Relic.type;
            OnRelicSelecting?.Invoke(selecting);
            description.Display(relic.Relic);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (selecting == null || eventData.button != PointerEventData.InputButton.Right) return;
            
            selecting = null;
            description.Undisplay();
        }

        public void SelectRelic()
        {
            ArmyDesign.Ins.SelectRelic((RelicType)selecting);
            relicText.text = description.nameText.text;
            Toggle();
        }
    }
}