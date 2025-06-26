using Core;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Color = Core.Color;

namespace Board.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piece : MonoBehaviour
    {
        private int rank;
        private int file;
        public Color side;
        public PieceType type;
        public void Spawn(int r, int f, PieceType t, Color s)
        {
            rank = r;
            file = f;
            type = t;
            side = s;

            transform.position = new Vector3(rank, 1, file);
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        
        public void Move(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;
            
            transform.position = new Vector3(rank, 1, file);
        }
    }
}
