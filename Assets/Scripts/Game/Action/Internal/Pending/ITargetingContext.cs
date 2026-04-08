using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UX.UI.Toolkit.Common;

namespace Game.Action.Internal.Pending
{
    public class MenuOption<T>
    {
        public string DisplayName;
        // public Sprite Icon; // Uncomment if you want to pass icons to the UI
        public T Value;
    }

    public interface ITargetingContext
    {
        UniTask<int> NextSelection(Func<int, bool> validator);
        UniTask<TResult> OpenMenu<TResult, TPayload>(InGameMenuType menuType, TPayload payload);
        
        void Highlighter(IEnumerable<int> positions);
        void ClearHighlights();
        void ClearHighlight(int position);
    }
}