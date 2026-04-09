using System;
using Cysharp.Threading.Tasks;
using Game.Action.Internal.Pending.Piece;
using Game.Managers;
using Game.Piece;
using UnityEngine.UIElements;
using UX.UI.Toolkit.Common;
using ZLinq;

namespace UX.UI.Toolkit.Ingame.SubMenus
{
    [UxmlElement]
    public partial class ChrysosShop : VisualElement, IAwaitableUI<ShopPayLoad, string>
    {
        private UniTaskCompletionSource<string> _tcs;
        private ScrollView _choiceList;
        private Button _confirm;

        private string _chosen;
        private VisualElement _currentlySelected;

        public UniTask<string> WaitForSelection(ShopPayLoad payload)
        {
            _tcs = new UniTaskCompletionSource<string>();
            _choiceList = this.Q<ScrollView>("ChrysosChoiceList");
            this.Q<Button>("ChrysosClose").clicked += Cancel;
            _confirm = this.Q<Button>("ChrysosConfirm");
            _confirm.SetEnabled(false);

            this.Q<Label>("UpgradeTo").text =
                Localizer.GetText("game", $"rank_{payload.UpgradableTo.ToString().ToLower()}", null);
            this.Q<Label>("Cost").text = payload.Cost.ToString();
            this.Q<Label>("Balance").text = payload.CurrentBalance.ToString();

            var upgradableTo = (from piece in AssetManager.Ins.PieceData.Values
                where piece.rank == payload.UpgradableTo
                select piece.key).ToList();

            if (payload.UpgradeFrom == PieceRank.Champion)
                upgradableTo.Remove(payload.CurrentPiece);

            if (!UIHolder.Ins.Get(InGameMenuType.ChrysosShopItem, out var chrysosShopItem))
            {
                throw new Exception("No Chrysos Shop Item found.");
            }

            _choiceList.Clear();
            _chosen = null;
            _currentlySelected = null;
            _confirm.SetEnabled(false);

            foreach (var id in upgradableTo)
            {
                var element = chrysosShopItem.Instantiate();
                var data = AssetManager.Ins.PieceData[id];

                element.Q<Label>("Name").text = Localizer.GetText("piece_name", data.key, null);

                element.Q<VisualElement>("SingleIcon").style.backgroundImage = new StyleBackground(data.icon);

                element.RegisterCallback<ClickEvent>(_ => OnItemClicked(element, id));

                _choiceList.Add(element);
            }

            _confirm.clicked += Upgrade;

            return _tcs.Task;
        }

        public void Confirm(string result)
        {
            _tcs.TrySetResult(result);
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

        private void Upgrade()
        {
            if (_chosen != null)
            {
                Confirm(_chosen);
            }
        }

        public void Cancel()
        {
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