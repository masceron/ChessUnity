using Game.Data.Pieces;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Followers
{
    public class PieceLogo: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] public UIObject3D model;
        private PieceObject obj;

        public void Load(PieceObject piece)
        {
            obj = piece;
            model.ObjectPrefab = piece.prefab.transform;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Followers.Ins.DisplayInfo((int)obj.type);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Followers.Ins.DisplayInfo(-1);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Followers.Ins.Select(obj.type);
            }
        }
    }
}