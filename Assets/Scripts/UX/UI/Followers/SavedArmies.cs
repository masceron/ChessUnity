using System.Linq;
using Game.Save.Army;
using UnityEngine;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SavedArmies: MonoBehaviour
    {
        [SerializeField] private RectTransform list;
        [SerializeField] private SavedArmy saved;

        public void Load()
        {
            var dict = ArmySaveLoader.ReadAll();
            
            var already = list.transform.childCount;
            var needed = dict.Count;
            
            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++)
                {
                    Instantiate(saved, list.transform, true);
                }
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
                list.transform.GetChild(i).GetComponent<SavedArmy>().Load(vlist[i]);
            }
        }
    }
}