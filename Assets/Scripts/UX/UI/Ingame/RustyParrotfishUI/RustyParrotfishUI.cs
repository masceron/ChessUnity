using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using PrimeTween;
using System.Collections.Generic;
using Game.Action.Internal.Pending;
using UnityEngine;

namespace UX.UI.Ingame.RustyParrotfishUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class RustyParrotfishUI : IngamePendingMenu
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject formationItem;
        private readonly List<int> _formationList = new();
        private int _caller;
        private int _to;

        private void OnEnable()
        {
            var rect = (RectTransform)transform.GetChild(0);
            rect.anchoredPosition = new Vector2(-50, 0);
            Tween.UIAnchoredPosition(rect, Vector3.zero, 0.3f);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ((RectTransform)transform.GetChild(0)).anchoredPosition = new Vector2(-50, 0);
        }

        public void Load(int caller, int to, PendingAction pd)
        {
            _caller = caller;
            _to = to;
            PendingAction = pd;
            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                if (BoardUtils.HasFormation(i))
                {
                    _formationList.Add(i);
                }
            }

            for (var i = 0; i < _formationList.Count; ++i)
            {
                Debug.Log("formation length: " + _formationList.Count);
                var formation = BoardUtils.GetFormation(_formationList[i]);
                Instantiate(formationItem, chooseField.transform, true);

                chooseField.transform.GetChild(i).GetComponent<RustyParrotfishItem>().Load(formation.GetFormationType().ToString());
            }
        }

        public void EraseFormation(int idx)
        {
            PendingAction.CommitResult(new RustyParrotfishActive(_caller, _to));
            FormationManager.Ins.RemoveFormation(_formationList[idx]);
            Disable();
        }

        protected override PendingAction PendingAction { get; set; }
    }
}