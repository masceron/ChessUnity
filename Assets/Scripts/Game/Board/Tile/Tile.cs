using Game.Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using static Game.Common.BoardUtils;

namespace Game.Board.Tile
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private int rank;
        private int file;
        private GameObject cell;
        private MeshRenderer floor;
        
        public void Spawn(int r, int f, GameObject prefab)
        {
            rank = r;
            file = f;
            cell = Instantiate(prefab, transform);
            cell.transform.position = new Vector3(rank, 1, file);
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
                default:
                    return;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            BoardInteractionUtils.Hover(IndexOf(rank, file));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            BoardInteractionUtils.Hover(-1);
        }
    }
}
