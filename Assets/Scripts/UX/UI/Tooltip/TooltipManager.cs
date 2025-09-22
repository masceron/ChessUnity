using Game.Common;

namespace UX.UI.Tooltip
{
    public class TooltipManager: Singleton<TooltipManager>
    {
        public Tooltip tooltip;

        public void Disable()
        {
            enabled = false;
            Hide();
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Show(string hleft, string hright, string cnt)
        {
            if (!enabled) return;
            
            tooltip.gameObject.SetActive(true);
            tooltip.SetText(hleft, hright, cnt);
        }

        public void Hide()
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}