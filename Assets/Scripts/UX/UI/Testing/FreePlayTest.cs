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

        public async UniTask<Empty> WaitForSelection(Empty payload)
        {
            _tcs = new UniTaskCompletionSource<Empty>();
            this.Q<Button>("FPTClose").clicked += ForceClose;


            await this.AnimateIn("popup--hidden", "popup--visible", 180);
            return await _tcs.Task;
        }

        public void Confirm(Empty result)
        {
            _tcs.TrySetResult(new Empty());
        }

        public async void Cancel()
        {
            await this.AnimateOut("popup--visible", 180);
            _tcs.TrySetResult(new Empty());
        }
    }
}