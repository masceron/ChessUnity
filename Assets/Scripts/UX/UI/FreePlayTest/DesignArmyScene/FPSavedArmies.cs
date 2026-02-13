using System.Collections.Generic;
using Game.Save.FreePlay;
using UnityEngine;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPSavedArmies : MonoBehaviour
    {
        [SerializeField] protected RectTransform list;
        [SerializeField] protected FreePlaySavedArmy saved;
        public List<FreePlaySavedArmy> savedArmies;
        public static FPSavedArmies Ins { get; private set; }

        protected void Awake()
        {
            Ins = this;
            Load();
        }

        public void Load()
        {
            var dict = FreePlaySaveLoader.ReadAll();

            var already = list.transform.childCount;
            var needed = dict.Count;

            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++) Instantiate(saved, list.transform, true);
            }
            else if (already > needed)
            {
                var index = already - 1;
                for (var i = 1; i <= already - needed; i++)
                {
                    Destroy(list.transform.GetChild(index).gameObject);
                    index--;
                }
            }

            var vlist = dict.Values.ToList();
            for (var i = 0; i < needed; i++)
            {
                var f = list.transform.GetChild(i).GetComponent<FreePlaySavedArmy>();
                f.Load(vlist[i]);
                savedArmies.Add(f);
            }
        }
    }
}