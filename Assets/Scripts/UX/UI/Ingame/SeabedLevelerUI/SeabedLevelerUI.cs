using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using PrimeTween;
using UnityEngine;

namespace UX.UI.Ingame.SeabedLevelerUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SeabedLevelerUI : MonoBehaviour
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject formationItem;
        private List<int> formationList = new();


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

        public void Load()
        {
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

                chooseField.transform.GetChild(i).GetComponent<SeabedLevelerItem>().Load(formation.GetFormationType().ToString());
            }
        }

        public void EraseFormation(int idx)
        {
            FormationManager.Ins.RemoveFormation(formationList[idx]);
            Disable();

        }
    }
}