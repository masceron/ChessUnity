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
        
        
        private int _maxFile;
        private int _maxRank;
    
        public void Spawn(int maxRank, int maxFile, PieceData[] config)
        {
            _maxFile = maxFile;
            _maxRank = maxRank;
            pieces = new Piece[maxRank * maxFile];
            
            for (var i = 0; i < maxRank * maxFile; i++)
            {
                if (config[i] == null) continue;
                switch (config[i].Type)
                {
                    case PieceType.Velkaris:
                        SpawnPiece(i, config[i].Color, 0, PieceType.Velkaris, new Velkaris());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SpawnPiece(int pos, Color side, int typeAsFab, PieceType typeAsEnum, IPieceLogic l)
        {
            var prefab = side == Color.White ? whitePiecePrefabs[typeAsFab] : blackPiecePrefabs[typeAsFab];
            var p = Instantiate(prefab).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(pos / _maxRank, pos % _maxFile, typeAsEnum, side, l, prefab);
        }
        
        public Piece GetPiece(int pos)
        {
            return pieces[pos];
        }

        public void Move(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
            pieces[to].Move(to / _maxFile, to % _maxFile);
        }
    }
}
