using UnityEngine;

namespace BoardLogic
{
    public class Pillar : MonoBehaviour
    {
        private int _maxTileNum;
        private int _posX;
        private int _posY;
        private bool _active;
        private float _size;
        private Vector3 _desired;
        private bool _moving;
        private Board _board;

        public void Set(int row, int col, bool active)
        {
            _posX = row;
            _posY = col;
            _active = active;
        }
    
        private void Start()
        {
            _board = transform.parent.parent.GetComponent<Board>();
            _size = _board.TileSize;
            transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            transform.position = _active ? new Vector3((_posX + 0.5f) * _size, -12.53f, (_posY + 0.5f) * _size) : new Vector3((_posX + 0.5f) * _size, -23.0f, (_posY + 0.5f) * _size);
            _desired = transform.position;
            _maxTileNum = _board.maxTileNum;
        }

        public void Activate()
        {
            _active = true;
            _moving = true;
            _desired = new Vector3((_posX + 0.5f) * _size, -12.53f, (_posY + 0.5f) * _size);
        }

        public void Deactivate()
        {
            _active = false;
            _moving = true;
            _desired = new Vector3((_posX + 0.5f) * _size, -23.0f, (_posY + 0.5f) * _size);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_moving) return;
        
            if (_desired != transform.position)
                transform.position = Vector3.MoveTowards(transform.position, _desired, 15 * Time.deltaTime);
            else
            {
                _moving = false;
                if (_active) _board.ActivateTile(_posX * _maxTileNum + _posY);
            }
        }
    }
}
