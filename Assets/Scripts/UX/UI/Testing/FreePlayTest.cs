using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using UX.UI.Common;

namespace UX.UI.Testing
{
    [UxmlElement]
    public partial class FreePlayTest: VisualElement, IAwaitableUI<Empty, Empty>
    {
        private UniTaskCompletionSource<Empty> _tcs;
        
        public void ForceClose()
        {
            Cancel();
        }

        public UniTask<Empty> WaitForSelection(Empty payload)
        {
            _tcs = new UniTaskCompletionSource<Empty>();
            this.Q<Button>("FPTClose").clicked += ForceClose;


            return _tcs.Task;
        }

        public void Confirm(Empty result)
        {
            _tcs.TrySetResult(new Empty());
        }

        public void Cancel()
        {
            _tcs.TrySetResult(new Empty());
        }
    }
}