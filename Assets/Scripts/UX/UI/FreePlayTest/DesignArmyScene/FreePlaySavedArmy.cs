using UX.UI.Followers;
using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest
{
    public class FreePlaySavedArmy : SavedArmy
    {
        public override void Click()
        {
            ArmyDesign.Ins.Load(army.BoardSize, army);
        }
    }
}

