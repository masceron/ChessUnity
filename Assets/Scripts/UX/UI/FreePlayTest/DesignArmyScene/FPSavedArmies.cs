using UX.UI.Followers;
using System.Collections.Generic;
using UnityEngine;
namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPSavedArmies : SavedArmies
    {
        public new static FPSavedArmies Ins;
        protected List<SavedArmy> savedArmies;
        protected override void Awake()
        {
            Ins = this;
        }
        public List<SavedArmy> GetList()
        {
            savedArmies = new();
            foreach (Transform tr in list)
            {
                savedArmies.Add(tr.GetComponent<FreePlaySavedArmy>());
            }
            return savedArmies;
        }
    }
}