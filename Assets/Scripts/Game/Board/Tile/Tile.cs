using Game.Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using static Game.Common.BoardUtils;
using Color = Game.Board.General.Color;

namespace Game.Board.Tile
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private int rank;
        private int file;
        [SerializeField] public Color color;
        
        public void Spawn(int pos)
        {
            rank = RankOf(pos);
            file = FileOf(pos);
            
            transform.position = new Vector3(rank, 1, file);
        }

        public void OnPointerClick(PointerEventData data)
        {
            switch (data.button)
            {
                case PointerEventData.InputButton.Left:
                    BoardInteractionUtils.MarkPiece(IndexOf(rank, file));
                    break;
                case PointerEventData.InputButton.Right:
                    BoardInteractionUtils.Unmark();
                    break;
                case PointerEventData.InputButton.Middle:
                default:
                    return;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (color != Color.None)
                BoardInteractionUtils.Hover(IndexOf(rank, file));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (color != Color.None) 
                BoardInteractionUtils.Hover(-1);
        }
    }
}
