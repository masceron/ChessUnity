using UnityEngine;

namespace BoardLogic
{
    public class Pieces : MonoBehaviour
    {
        private Piece[] _piecesArr;
        private GameObject[][] _piecePrefabs;
        private int _maxTileNumX;
        private int _maxTileNumY;
        
        [SerializeField] private GameObject[] prefabsWhite;
        [SerializeField] private GameObject[] prefabsBlack;

        public void Init(int maxX, int maxY)
        {
            _maxTileNumX = maxX;
            _maxTileNumY = maxY;
            _piecePrefabs = new GameObject[2][];
            _piecePrefabs[0] = prefabsWhite;
            _piecePrefabs[1] = prefabsBlack;
            _piecesArr = new Piece[_maxTileNumX * _maxTileNumY];
        }

        public void Select(int index)
        {
            _piecesArr[index].Select();
        }

        public void Unselect(int index)
        {
            _piecesArr[index].Unselect();
        }

        public void Set(int index, Piece piece)
        {
            _piecesArr[index] = piece;
        }

        public Piece Get(int index)
        {
            return _piecesArr[index];
        }

        public void SpawnPiece(PieceSide pieceSide, PieceType piece, int x, int y)
        {
            var p = Instantiate(_piecePrefabs[(int)pieceSide][(int)piece], transform).AddComponent<Piece>();
            p.Set(piece, pieceSide);
            p.position = new Vector2Int(x, y);

            _piecesArr[x * _maxTileNumY + y] = p;
        }
    }
    
    
}
