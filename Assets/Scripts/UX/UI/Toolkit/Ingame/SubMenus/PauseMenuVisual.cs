using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using UX.UI.Toolkit.Common;

namespace UX.UI.Toolkit.Ingame.SubMenus
{
    [UxmlElement]
    public partial class PauseMenuVisual: VisualElement, IAwaitableUI<Empty, Empty>
    {
        private UniTaskCompletionSource<Empty> _tcs;
        private VisualElement _mainPanel;
        private const int AnimationTime = 250;
        private Button _resume;
        private Button _quit;
        
        public void ForceClose()
        {
            if (_tcs is { Task: { Status: UniTaskStatus.Pending } })
            {
                CloseMenuSequence().Forget();
            }
        }

        public async UniTask<Empty> WaitForSelection(Empty payload)
        {
            _resume = this.Q<Button>("Resume");
            _quit = this.Q<Button>("Quit");
            _resume.SetEnabled(false);
            _quit.SetEnabled(false);
            
            _tcs = new UniTaskCompletionSource<Empty>();

            _resume.clicked += Cancel;
            _mainPanel = this.Q<VisualElement>("MainPanel");
            
            await UniTask.DelayFrame(2);
            _mainPanel.AddToClassList("pause-side-panel--visible");
            await UniTask.Delay(AnimationTime + 30, ignoreTimeScale: true);
            
            _resume.SetEnabled(true);
            _quit.SetEnabled(true);
            
            return await _tcs.Task;
        }
        
        private async UniTaskVoid CloseMenuSequence()
        {
            this.Q<VisualElement>("MainContainer").style.display = DisplayStyle.None;
            _mainPanel.RemoveFromClassList("pause-side-panel--visible");
            
            await UniTask.Delay(AnimationTime + 30, ignoreTimeScale: true);
            
            _tcs.TrySetResult(new Empty());
        }

        public void Confirm(Empty result)
        {
            ForceClose();
        }

        public void Cancel()
        {
            ForceClose();
        }
    }
}