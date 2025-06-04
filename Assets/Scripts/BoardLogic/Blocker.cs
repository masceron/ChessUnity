using UnityEngine;

namespace BoardLogic
{
    public class Blocker : MonoBehaviour
    {
        private int _row;
        private int _col;
        private float _tileSize;

        private void Start()
        {
            _tileSize = transform.parent.parent.GetComponent<Board>().TileSize;
            transform.position = new Vector3((_row + 0.5f) * _tileSize, 1, (_col + 0.5f) * _tileSize);
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }

        public void Set(int row, int col)
        {
            _row = row;
            _col = col;
        }

        private void Update()
        {
            transform.Rotate(0f, 35 * Time.deltaTime, 0f);
        }
    }
}
