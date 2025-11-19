using Game.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using static Game.Common.BoardUtils;
using Color = Game.Managers.Color;

namespace Game.Tile
{


    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public int rank;
        public int file;
        public Formation tileEffect;
        [SerializeField] public Color color;

        // đỉnh trong ô 
        public Corner corner { get; private set; }
        public delegate void OnPointerEnterHandler(Tile thisTile);

        public static OnPointerEnterHandler OnPointEnterHandle;
        public void Start()
        {
            if (color == Color.None) gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void Spawn(int pos)
        {
            rank = RankOf(pos);
            file = FileOf(pos);
            
            transform.position = new Vector3(rank, 1, file);
        }

        public void OnPointerClick(PointerEventData data)
        {
            
            var processor = MatchManager.Ins.InputProcessor;
            if (!processor) return;
            switch (data.button)
            {
                case PointerEventData.InputButton.Left:
                    processor.MarkPiece(IndexOf(rank, file));
                    break;
                case PointerEventData.InputButton.Right:
                    processor.Unmark();
                    break;
                case PointerEventData.InputButton.Middle:
                default:
                    return;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            corner = TileManager.Ins.IndexToCorner(eventData.pointerCurrentRaycast.worldPosition, this);

            if (MatchManager.Ins.InputProcessor)
                MatchManager.Ins.InputProcessor.Hover(IndexOf(rank, file));

            OnPointEnterHandle?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (MatchManager.Ins.InputProcessor) 
                MatchManager.Ins.InputProcessor.Hover(-1);
        }
    }
}
