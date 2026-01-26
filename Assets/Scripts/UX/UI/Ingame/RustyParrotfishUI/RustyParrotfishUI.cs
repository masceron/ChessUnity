using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Tile;
using PrimeTween;
using System.Collections.Generic;
using UnityEngine;

namespace UX.UI.Ingame.RustyParrotfishUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class RustyParrotfishUI : MonoBehaviour
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject formationItem;
        private List<int> formationList = new();
        private int caller;
        private int to;

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

        public void Load(int caller, int to)
        {
            this.caller = caller;
            this.to = to;
            for (int i = 0; i < BoardUtils.BoardSize; ++i)
            {
                if (FormationManager.Ins.HasFormation(i))
                {
                    formationList.Add(i);
                }
            }

            for (int i = 0; i < formationList.Count; ++i)
            {
                Debug.Log("formation length: " + formationList.Count);
                var formation = FormationManager.Ins.GetFormation(formationList[i]);
                Instantiate(formationItem, chooseField.transform, true);

                chooseField.transform.GetChild(i).GetComponent<RustyParrotfishItem>().Load(formation.GetFormationType().ToString());
            }
        }

        public void EraseFormation(int idx)
        {
            MatchManager.Ins.InputProcessor.ExecuteAction(new RustyParrotfishActive(caller, to));
            FormationManager.Ins.RemoveFormation(formationList[idx]);
            Disable();
        }
    }
}