using System;
using Core;
using Core.PieceLogic;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Color = Core.Color;

namespace Board.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : MonoBehaviour
    {
        private Piece[] pieces;
        
        [SerializeField] private GameObject[] whitePiecePrefabs;
        [SerializeField] private GameObject[] blackPiecePrefabs;
        
        
        private int maxFile;
        private int maxRank;
    
        public void Spawn(int r, int f, PieceData[] config)
        {
            maxFile = r;
            maxRank = f;
            pieces = new Piece[maxRank * maxFile];
            
            for (var i = 0; i < maxRank * maxFile; i++)
            {
                if (config[i] == null) continue;
                switch (config[i].Type)
                {
                    case PieceType.Velkaris:
                        SpawnPiece(i, 0, PieceType.Velkaris, new Velkaris(config[i]), config[i]);
                        break;
                    case PieceType.GuidingSiren:
                        SpawnPiece(i, 1, PieceType.GuidingSiren, new GuidingSiren(config[i]), config[i]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SpawnPiece(int pos, int typeAsFab, PieceType typeAsEnum, PieceLogic l, PieceData data)
        {
            var prefab = data.Color == Color.White ? whitePiecePrefabs[typeAsFab] : blackPiecePrefabs[typeAsFab];
            var p = Instantiate(prefab).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(pos / maxRank, pos % maxFile, typeAsEnum, data.Color, l, prefab);
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
