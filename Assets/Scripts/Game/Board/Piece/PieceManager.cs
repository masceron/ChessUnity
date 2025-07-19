using System.Collections.Generic;
using Game.Board.General;
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
    
        public void Init(List<PieceConfig> config, Dictionary<PieceType, PieceObject> dict)
        {
            piecesInfo = dict;
            pieces = new Piece[BoardSize];

            foreach (var piece in config)
            {
                SpawnPiece(piece);
            }
        }

        private void SpawnPiece(PieceConfig config)
        {
            var pos = config.Index;
            var info = piecesInfo[config.Type];
            var prefab = info.prefab;
            var p = Instantiate(prefab).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(RankOf(pos), FileOf(pos), info.defaultTransform);
            MatchManager.GameState.SpawnPiece(config);
        }
        
        public Piece GetPiece(int pos)
        {
            return pieces[pos];
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
