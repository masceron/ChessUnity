using UX.UI.Followers;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FreePlaySavedArmies : SavedArmies
    {
        protected override void Awake()
        {
            base.Awake();
            Load();
        }

    }
}