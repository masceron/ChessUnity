using UnityEngine.UIElements;

namespace UX.UI.Common.Tooltip
{
    public static class TooltipExtensions
    {
        public static TooltipManipulator AddTooltip(this VisualElement element, string firstHeader, string secondHeader,
            string content)
        {
            var manipulator = new TooltipManipulator(firstHeader, secondHeader, content);
            element.AddManipulator(manipulator);
            return manipulator;
        }

        public static void RemoveTooltip(this VisualElement element, TooltipManipulator manipulator)
        {
            element.RemoveManipulator(manipulator);
        }
    }
}