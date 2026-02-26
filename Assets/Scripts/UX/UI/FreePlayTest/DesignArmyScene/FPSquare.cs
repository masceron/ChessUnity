using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.FreePlayTest.DesignArmyScene
{
    public class FPSquare : MonoBehaviour, IDropHandler
    {
        private static readonly Color White = new(0.9411765f, 0.8509804f, 0.7098039f, 1);
        private static readonly Color Black = new(0.7098039f, 0.5333334f, 0.3882353f, 1);
        [SerializeField] private RectTransform rect;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private Image image;
        private bool color;
        private int file;
        private int rank;

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent<FreePlayArmyTroop>(out var troopDisplay)) return;
            var board = FindAnyObjectByType<FreePlayArmyBoard>();

            if (!board.IsAllowed(rank, file)) return;

            troopDisplay.Parent = transform;
            if (troopDisplay.Placed)
            {
                board.Move(troopDisplay.Rank, troopDisplay.File, rank, file);
                troopDisplay.Set(rank, file);
            }
            else
            {
                board.Add(rank, file, troopDisplay.Piece.key);
                troopDisplay.Set(rank, file);
            }
        }

        public void SetSquare(int r, int f, float size, bool c)
        {
            rank = r;
            file = f;
            grid.cellSize = new Vector2(size, size);
            image.color = c ? Black : White;
            color = c;
        }

        public void MarkAsNotAllowed()
        {
            var imageColor = image.color;
            imageColor.a = 0.5f;
            image.color = imageColor;
        }

        public void UnMark()
        {
            var imageColor = image.color;
            imageColor.a = 1;
            image.color = imageColor;
        }
    }
}