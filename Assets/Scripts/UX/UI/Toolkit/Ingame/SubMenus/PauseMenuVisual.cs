using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Toolkit.Common;
using UX.UI.Toolkit.Loader;

namespace UX.UI.Toolkit.Ingame.SubMenus
{
    [UxmlElement]
    public partial class PauseMenuVisual: VisualElement, IAwaitableUI<UIDocument, Empty>
    {
        private UniTaskCompletionSource<Empty> _tcs;
        private VisualElement _mainPanel;
        private const int AnimationTime = 180;
        private Button _resume;
        private Button _quit;
        private Button _mainMenu;
        
        public void ForceClose()
        {
            if (_tcs is { Task: { Status: UniTaskStatus.Pending } })
            {
                CloseMenuSequence().Forget();
            }
        }

        public async UniTask<Empty> WaitForSelection(UIDocument payload)
        {
            payload.GetComponent<UIDocumentBlur>().RefreshPanels();
            _resume = this.Q<Button>("Resume");
            _quit = this.Q<Button>("Quit");
            _mainMenu = this.Q<Button>("MainMenu");
            
            _resume.SetEnabled(false);
            _mainMenu.SetEnabled(false);
            _quit.SetEnabled(false);
            
            _tcs = new UniTaskCompletionSource<Empty>();

            _resume.clicked += Cancel;
            _mainMenu.clicked += () =>
            {
                SceneLoader.Ins.ChangeScene("Main Menu");
            };
            _quit.clicked += () => Application.Quit(0);
            _mainPanel = this.Q<VisualElement>("MainPanel");
            
            await UniTask.DelayFrame(2);
            _mainPanel.AddToClassList("pause-side-panel--visible");
            await UniTask.Delay(AnimationTime, ignoreTimeScale: true);
            
            _resume.SetEnabled(true);
            _mainMenu.SetEnabled(true);
            _quit.SetEnabled(true);
            
            return await _tcs.Task;
        }
        
        private async UniTaskVoid CloseMenuSequence()
        {
            this.Q<VisualElement>("MainContainer").style.display = DisplayStyle.None;
            _mainPanel.RemoveFromClassList("pause-side-panel--visible");
            
            await UniTask.Delay(AnimationTime, ignoreTimeScale: true);
            
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