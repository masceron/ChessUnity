using Game.Managers;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UX.UI.Ingame.DormantFossil;

namespace UX.UI.Ingame.RustyParrotfishUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RustyParrotfishItem : MonoBehaviour
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
            transform.parent.parent.GetComponent<RustyParrotfishUI>().EraseFormation(transform.GetSiblingIndex());
            Destroy(gameObject);
        }
    }
}