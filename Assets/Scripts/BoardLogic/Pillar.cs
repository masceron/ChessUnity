using UnityEngine;

namespace BoardLogic
{
    public class Pillar : MonoBehaviour
    {
        public int posX;
        public int posY;
        public bool active;
        public float size;
        private Vector3 _desired;
        private bool _moving;
        private Board _board;
    
        private void Start()
        {
            _board = transform.parent.parent.GetComponent<Board>();
            transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            transform.position = active ? new Vector3((posX + 0.5f) * size, -12.53f, (posY + 0.5f) * size) : new Vector3((posX + 0.5f) * size, -23.0f, (posY + 0.5f) * size);
            _desired = transform.position;
        }

        public void Activate()
        {
            active = true;
            _moving = true;
            _desired = new Vector3((posX + 0.5f) * size, -12.53f, (posY + 0.5f) * size);
        }

        public void Deactivate()
        {
            active = false;
            _moving = true;
            _desired = new Vector3((posX + 0.5f) * size, -23.0f, (posY + 0.5f) * size);
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
                if (active) _board.ActivateTile(posX, posY);
            }
        }
    }
}
