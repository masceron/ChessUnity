using Game.Common;

namespace UX.UI.Ingame.Tooltip
{
    public class TooltipManager: Singleton<TooltipManager>
    {
        public Tooltip tooltip;
        
        public void Show(string hleft, string hright, string cnt)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.SetText(hleft, hright, cnt);
        }

        public void Hide()
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}