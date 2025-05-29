using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Color _startColor;
        private Renderer _renderer;
        private bool _selected;
        private Board _board;
        public int x;
        public int y;

        private void Start()
        {
            _board = transform.parent.parent.GetComponent<Board>();
            _selected = false;
            _renderer = GetComponent<MeshRenderer>();
            _startColor = _renderer.material.color;
        }
    
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (!_selected)
                _renderer.material.color = new Color(0.67f, 0.64f, 0.23f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_selected)
                _renderer.material.color = _startColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _board.Select(x, y);
        }

        public void Select()
        {
            _selected = true;
            _renderer.material.color = new Color(0.8f, 0.83f, 0.42f);
        }
    
        public void Unselect()
        {
            _renderer.material.color = _startColor;
            _selected = false;
        }
    }
}
