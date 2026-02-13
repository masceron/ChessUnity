#region Namespace Imports

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace UI.UIObject3D.Scripts
{
    public static class UIObject3DUtilities
    {
        private static readonly Dictionary<UIObject3D, Vector3> targetContainers = new();

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        public static Vector3 NormalizeRotation(Vector3 rotation)
        {
            return new Vector3(NormalizeAngle(rotation.x), NormalizeAngle(rotation.y), NormalizeAngle(rotation.z));
        }

        private static float NormalizeAngle(float value)
        {
            value %= 360;

            if (value < 0) value += 360;

            return value;
        }

        internal static void RegisterTargetContainerPosition(UIObject3D uiObject3D, Vector3 position)
        {
            targetContainers.TryAdd(uiObject3D, position);
        }

        internal static Vector3 GetTargetContainerPosition(UIObject3D uiObject3d)
        {
            return targetContainers.TryGetValue(uiObject3d, out var container)
                ? container
                : GetNextFreeTargetContainerPosition();
        }

        private static Vector3 GetNextFreeTargetContainerPosition()
        {
            if (!targetContainers.Any()) return Vector3.zero;

            var lastXInUse = targetContainers.Max(v => v.Value.x);

            return new Vector3(lastXInUse + 250f, 0f, 0f);
        }

        internal static void UnRegisterTargetContainer(UIObject3D uiObject3D)
        {
            targetContainers.Remove(uiObject3D);
        }
    }
}