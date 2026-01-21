using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece;
using PrimeTween;
using UnityEngine;

namespace UX.UI.Ingame.SeabedLevelerUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SeabedLevelerUI : MonoBehaviour
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject formationItem;


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
            for (int i = 0; i < FormationManager.Ins.GetAllFormations().Count; ++i)
            {
                Debug.Log("formation length: " + FormationManager.Ins.GetAllFormations().Count);
                var formation = FormationManager.Ins.GetFormation(i);
                Instantiate(formationItem, chooseField.transform, true);

                Debug.Log(formation.GetFormationType().ToString());
                chooseField.transform.GetChild(i).GetComponent<SeabedLevelerItem>().Load(formation.GetFormationType().ToString());
                
            }
        }

        public void EraseFormation(int idx)
        {
            FormationManager.Ins.RemoveFormation(idx);
        }
    }
}