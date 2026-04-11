using System;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Managers;
using UnityEngine.UIElements;
using UX.UI.Common;

namespace UX.UI.Ingame.SubMenus
{
    [UxmlElement]
    public partial class ThalassosShop: VisualElement, IAwaitableUI<bool, string>
    {
        private UniTaskCompletionSource<string> _tcs;
        private ScrollView _choiceList;
        private Button _confirm;
        private string _chosen;
        private VisualElement _currentlySelected;
        
        public async UniTask<string> WaitForSelection(bool payload)
        {
            _tcs = new UniTaskCompletionSource<string>();
            
            _choiceList = this.Q<ScrollView>("ThalassosChoiceList");
            this.Q<Button>("ThalassosClose").clicked += Cancel;
            _confirm = this.Q<Button>("ThalassosConfirm");
            _confirm.SetEnabled(false);

            if (!UIHolder.Ins.Get(InGameMenuType.ThalassosItem, out var thalassosItem))
            {
                throw new Exception("No Thalassos Shop Item found.");
            }
            
            _choiceList.Clear();
            _chosen = null;
            _currentlySelected = null;
            _confirm.SetEnabled(false);
            
            foreach (var id in BoardUtils.GetCapturedOf(payload))
            {
                var element = thalassosItem.Instantiate();
                var data = AssetManager.Ins.PieceData[id.Type];

                element.Q<Label>("Name").text = Localizer.GetText("piece_name", data.key, null);

                element.Q<VisualElement>("SingleIcon").style.backgroundImage = new StyleBackground(data.icon);

                element.RegisterCallback<ClickEvent>(_ => OnItemClicked(element, id.Type));

                _choiceList.Add(element);
            }

            _confirm.clicked += Resurrect;
            
            await this.AnimateIn("popup--hidden", "popup--visible");

            return await _tcs.Task;
        }

        private void Resurrect()
        {
            if (_chosen != null) Confirm(_chosen);
        }
        
        private void OnItemClicked(VisualElement clickedElement, string id)
        {
            _currentlySelected?.Q<VisualElement>("SingleIcon").RemoveFromClassList("shop-item--selected");

            if (_currentlySelected == clickedElement)
            {
                _currentlySelected = null;
                Choose(null);
            }
            else
            {
                _currentlySelected = clickedElement;
                _currentlySelected.Q<VisualElement>("SingleIcon").AddToClassList("shop-item--selected");
                Choose(id);
            }
        }

        private void Choose(string id)
        {
            _chosen = id;
            _confirm.SetEnabled(id != null);
        }

        public async void Confirm(string result)
        {
            await this.AnimateOut("popup--visible");
            _tcs.TrySetResult(result);
        }

        public async void Cancel()
        {
            await this.AnimateOut("popup--visible");
            _tcs.TrySetResult(null);
        }

        public void ForceClose()
        {
            if (_tcs is { Task: { Status: UniTaskStatus.Pending } })
            {
                Cancel();
            }
        }
    }
}