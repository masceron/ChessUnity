using UnityEngine.UIElements;

namespace UX.UI.Common.Tooltip
{
    public class TooltipManipulator : PointerManipulator
    {
        private readonly string _headerLeft;
        private readonly string _headerRight;
        private readonly string _content;

        public TooltipManipulator(string left, string right, string cnt)
        {
            _headerLeft = left;
            _headerRight = right;
            _content = cnt;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            TooltipManager.Ins.HandlePointerEnter(target, _headerLeft, _headerRight, _content);
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            TooltipManager.Ins.HandlePointerLeave(target); 
        }
    }
}