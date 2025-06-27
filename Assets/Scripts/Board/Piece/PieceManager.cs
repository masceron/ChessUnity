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
            
            for (int i = 0; i < maxRank * maxFile; i++)
            {
                switch (config[i].Type)
                {
                    case PieceType.Velkaris:
                        SpawnPiece(i, config[i].Color, 0, PieceType.Velkaris, Color.White, new Velkaris());
                        break;
                    case PieceType.Nil:
                        pieces[i] = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SpawnPiece(int pos, Color side, int typeAsFab, PieceType typeAsEnum, Color c, IPieceLogic l)
        {
            var p = Instantiate(side == Color.White ? whitePiecePrefabs[typeAsFab] : blackPiecePrefabs[typeAsFab]).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(pos / _maxRank, pos % _maxFile, typeAsEnum, c, l);
        }
        
        public Piece GetPiece(int pos)
        {
            return pieces[pos];
        }

        public void Move(int from, int to)
        {
            pieces[to] = pieces[from];
            pieces[from] = null;
        }
    }
}
