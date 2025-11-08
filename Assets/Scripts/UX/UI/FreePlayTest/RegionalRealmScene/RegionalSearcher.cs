

using Game.Common;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Managers;
using LineupConfig = Game.Save.Stage.LineupConfig;
using Game.ScriptableObjects.Collections;
using Game.Effects.RegionalEffect;
using System;

namespace UX.UI.FreePlayTest
{
    public class RegionalSearcher : Singleton<RegionalSearcher>
    {
        public Transform list;
        public RegionalIcon regionalIcon;
        public readonly List<RegionalIcon> Pool = new();
        private List<RegionalEffectType> lastSearchResult;
        public void Load()
        {
            lastSearchResult = Enum.GetValues(typeof(RegionalEffectType)).Cast<RegionalEffectType>().ToList();
            lastSearchResult.Remove(RegionalEffectType.None);
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
                            Pool.Add(Instantiate(regionalIcon, list).GetComponent<RegionalIcon>());
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

                Pool[i].Load(lastSearchResult[i]);

            }
        }
    }
}