using Core;
using Core.PieceLogic;
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
        public IPieceLogic logic;
        public void Spawn(int r, int f, PieceType t, Color s, IPieceLogic l, GameObject p)
        {
            rank = r;
            file = f;
            type = t;
            side = s;
            logic = l;

            transform.position = new Vector3(rank, p.transform.position.y, file);
        }
        
        public void Move(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;
            
            transform.position = new Vector3(rank, transform.position.y, file);
        }
    }
}
