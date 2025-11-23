using UnityEngine.UI;
using UnityEngine;
using UX.UI.Followers;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlaySavedArmy : SavedArmy
    {
        [SerializeField] private Image image;
        public override void Click()
        {
            FreePlayArmyDesign.Ins.Load(army.BoardSize, army);
            foreach(FreePlaySavedArmy savedArmy in FPSavedArmies.Ins.GetList())
            {
                if(savedArmy != this)
                {
                    savedArmy.RemoveHighLight();
                }
            }
            image.color = Color.white;
        }
        public void RemoveHighLight()
        {
            image.color = new Color(229/255.0f, 232/255.0f, 69/255.0f);
        }
    }
}

