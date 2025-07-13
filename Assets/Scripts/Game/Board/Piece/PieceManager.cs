using System.Collections.Generic;
using Game.Board.General;
using UnityEngine;

namespace Game.Board.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : MonoBehaviour
    {
        private Piece[] pieces;
        
        private Dictionary<PieceType, PieceObject> piecesInfo;
        
        private int maxFile;
        private int maxRank;
    
        public void Init(int r, int f,  List<PieceConfig> config, Dictionary<PieceType, PieceObject> dict)
        {
            piecesInfo = dict;
            maxFile = r;
            maxRank = f;
            pieces = new Piece[maxRank * maxFile];

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
            p.Spawn(pos / maxRank, pos % maxFile, info.defaultTransform);
            MatchManager.GameState.SpawnPiece(config);
        }
        
        public Piece GetPiece(int pos)
        {
            return pieces[pos];
        }

        public void Move(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
            pieces[to].Move(to / maxFile, to % maxFile);
        }
    }
}
