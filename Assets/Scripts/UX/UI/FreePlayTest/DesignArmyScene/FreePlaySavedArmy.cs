using UX.UI.Army.DesignArmy;
using UX.UI.Followers;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FreePlaySavedArmy : SavedArmy
    {
        public override void Click()
        {
            ArmyDesign.Ins.Load(army.BoardSize, army);
        }
    }
}

