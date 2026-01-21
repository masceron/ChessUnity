using Game.Managers;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UX.UI.Ingame.DormantFossil;

namespace UX.UI.Ingame.SeabedLevelerUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLevelerItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text formationName;
        [SerializeField] private Sprite formationImage;
        private string formationType;

        public void Load(string type)
        {
            formationName.text = type;
        }

        public void OnClickErase()
        {
            transform.parent.parent.GetComponent<SeabedLevelerUI>().EraseFormation(transform.GetSiblingIndex());
        }
    }
}