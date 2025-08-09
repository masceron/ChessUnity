using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArmyDesignSquare: MonoBehaviour, IDropHandler
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private GridLayoutGroup grid;
        private int rank;
        private int file;
        
        public void SetSquare(int r, int f, float size)
        {
            rank = r;
            file = f;
            grid.cellSize = new Vector2(size, size);
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var type = eventData.pointerDrag.GetComponent<ArmyDesignTroop>();
            if (!type) return;
            
            if (FindAnyObjectByType<ArmyDesign>().IsAllowed(rank, file))
            {
                type.Parent = transform;
            }
        }
    }
}