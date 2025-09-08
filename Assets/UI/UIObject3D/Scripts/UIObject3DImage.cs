#region Namespace Imports

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.UIObject3D.Scripts
{
    /// <summary>
    /// Subclass of the Unity 'Image' component which avoids the default layout sizing behaviour
    /// </summary>
    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent, ExecuteInEditMode]    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIObject3DImage : Image, ILayoutElement
    {        
        void ILayoutElement.CalculateLayoutInputHorizontal()
        {            
        }

        void ILayoutElement.CalculateLayoutInputVertical()
        {            
        }

        float ILayoutElement.flexibleHeight => 1;

        float ILayoutElement.flexibleWidth => 1;

        int ILayoutElement.layoutPriority => -1;

        float ILayoutElement.minHeight => 0;

        float ILayoutElement.minWidth => 0;

        float ILayoutElement.preferredHeight => 0;

        float ILayoutElement.preferredWidth => 0;
    }
}
