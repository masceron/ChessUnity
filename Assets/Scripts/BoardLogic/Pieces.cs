using UnityEngine;

namespace BoardLogic
{
    public class Pieces : MonoBehaviour
    {
        public Piece[] piecesArr;
        public GameObject[][] PiecePrefabs;
        public int maxTileNum;
        
        public void SpawnPiece(Side side, PieceType piece, int x, int y)
        {
            var p = Instantiate(PiecePrefabs[(int)side][(int)piece], transform).AddComponent<Piece>();
            p.type = piece;
            p.side = side;
            p.position = new Vector2Int(x, y);

            piecesArr[x * maxTileNum + y] = p;
        }
    }
    
    
}
