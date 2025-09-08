using Game.ScriptableObjects;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TroopLogo: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] public UIObject3D model;
        private PieceInfo obj;

        public void Load(PieceInfo piece)
        {
            obj = piece;
            model.ObjectPrefab = piece.prefab.transform;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TroopList.Ins.DisplayInfo(obj.type);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TroopList.Ins.Undisplay();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                TroopList.Ins.Select(obj.type);
            }
        }
    }
}