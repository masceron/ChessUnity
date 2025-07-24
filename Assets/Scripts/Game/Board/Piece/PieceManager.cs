using System.Collections.Generic;
using UnityEngine;
using static Game.Common.BoardUtils;
using Object = UnityEngine.Object;

namespace Game.Board.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : MonoBehaviour
    {
        private Piece[] pieces;
        
        private Dictionary<PieceType, PieceObject> piecesInfo;
    
        public void Init(Dictionary<PieceType, PieceObject> dict)
        {
            piecesInfo = dict;
            pieces = new Piece[BoardSize];
        }

        public void SpawnPiece(PieceConfig config)
        {
            var pos = config.Index;
            var info = piecesInfo[config.Type];
            var prefab = info.prefab;
            var p = Instantiate(prefab).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(RankOf(pos), FileOf(pos), info.defaultTransform);
        }

        public void Destroy(int pos)
        {
            Object.Destroy(pieces[pos].gameObject);
            pieces[pos] = null;
        }

        public void Move(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
            pieces[to].Move(RankOf(to), FileOf(to));
        }
    }
}
