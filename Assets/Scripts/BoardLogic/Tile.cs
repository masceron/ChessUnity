using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Color _startColor;
        private readonly Color _expandColor = new Color(1f, 0.8431373f, 0f, 0.5f);
        private Renderer _renderer;
        private bool _selected;
        private bool _expand;
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
            {
                GetComponent<MeshRenderer>().material.color = _expand ? Color.gold : new Color(0.67f, 0.64f, 0.23f);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_selected)
            {
                _renderer.material.color = _expand ? _expandColor : _startColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _board.Select(x, y);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _board.Deactivate(x, y);
            }
        }

        public void Activate()
        {
            gameObject.layer = LayerMask.NameToLayer("Tile");
            _expand = false;
            Unselect(false);
        }

        public void Deactivate()
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            Unselect(false);
        }

        public void Select(bool isExpand)
        {
            _selected = true;
            _renderer.material.color = isExpand ? new Color(1f, 0.8431373f, 0f, 1f) : new Color(0.8f, 0.83f, 0.42f);
        }
    
        public void Unselect(bool isExpand)
        {
            _renderer.material.color = isExpand ? _expandColor : _startColor;
            _selected = false;
        }

        public void MarkAsExpandable()
        {
            _expand = true;
            _renderer.material.color = _expandColor;
        }

        public void UnmarkAsExpandable()
        {
            _expand = false;
            _renderer.material.color = _startColor;
        }
    }
}
