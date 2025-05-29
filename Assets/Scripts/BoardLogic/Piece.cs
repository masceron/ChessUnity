using System;
using UnityEngine;

namespace BoardLogic
{
    public enum PieceType {
        Pawn, Knight, Bishop, Rook, Queen, King
    }

    public enum Side {
        White, Black
    }

    [Serializable]
    public class PieceData
    {
        public int side;
        public int piece;
        public int[] pos;
    }

    [Serializable]
    public class PiecesData
    {
        public PieceData[] pieces;
    }


    public class Piece : MonoBehaviour
    {
        public PieceType type;
        public Side side;
        private Vector3 _desired;
        private bool _moving;
        private Outline _outline;
        private bool _selected;
    
        public Vector2Int position;
    
        private void Start()
        {
            _outline = transform.GetComponent<Outline>();
            _outline.OutlineColor = side == Side.White ? Color.black : Color.white;
            _outline.OutlineWidth = 3f;
            _outline.enabled = false;
        
            _moving = false;
            transform.position = new Vector3(1.5f * position.x + 0.75f, 0, 1.5f * position.y + 0.75f);
            _desired = transform.position;
        }

        public void Select()
        {
            _outline.enabled = true;
        }

        public void Unselect()
        {
            _outline.enabled = false;
        }

        public void Move(int xTo, int yTo)
        {
            Unselect();
            _desired = new Vector3(1.5f * xTo + 0.75f, 0, 1.5f * yTo + 0.75f);
            _moving = true;
        }
    
        private void Update()
        {
            if (!_moving) return;
        
            if (_desired != transform.position)
                transform.position = Vector3.MoveTowards(transform.position, _desired, 40 * Time.deltaTime);
            else _moving = false;
        }
    }
}