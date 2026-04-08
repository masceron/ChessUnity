using UnityEngine;
using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Common.Tooltip
{
    public class TooltipManipulator : PointerManipulator
    {
        private readonly string _headerLeft;
        private readonly string _headerRight;
        private readonly string _content;
        private IVisualElementScheduledItem _delayTask;

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
            _delayTask = target.schedule.Execute(() =>
            {
                TooltipManager.Ins.Show(_headerLeft, _headerRight, _content);
            }).StartingIn(1000);
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            _delayTask?.Pause(); 
            TooltipManager.Ins.Hide();
        }
    }
}