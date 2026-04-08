using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Common.Tooltip
{
    public static class TooltipExtensions
    {
        public static TooltipManipulator AddTooltip(this VisualElement element, string left, string right, string content)
        {
            var manipulator = new TooltipManipulator(left, right, content);
            element.AddManipulator(manipulator);
            return manipulator;
        }

        public static void RemoveTooltip(this VisualElement element, TooltipManipulator manipulator)
        {
            element.RemoveManipulator(manipulator);
        }
    }
}