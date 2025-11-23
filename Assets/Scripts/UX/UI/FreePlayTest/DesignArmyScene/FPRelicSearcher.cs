using UX.UI.Army.DesignArmy;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPRelicSearcher: ArmyRelicSearcher
    {
        public override void SelectRelic()
        {
            FreePlayArmyDesign.Ins.SelectRelic(selecting);
            relicText.text = description.nameText.text;
            Toggle();
        }
    }
}