using System;
using Board.Interaction;
using Core;
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
        [SerializeField] private GameObject selectionPrefab;

        private Selection[] selections;
        
        private int _maxFile;
        private int _maxRank;

        private void SelectionIndicator(int pos)
        {
            var sel = Instantiate(selectionPrefab, transform, true);
            sel.transform.position = new Vector3(pos / _maxRank, 1.15f, pos % _maxFile);
            sel.name = pos.ToString();
            sel.SetActive(false);
            
            selections[pos] = sel.AddComponent<Selection>();
        }
    
        public void Spawn(int maxRank, int maxFile, PieceType[] config)
        {
            _maxFile = maxFile;
            _maxRank = maxRank;
            pieces = new Piece[maxRank * maxFile];
            selections = new Selection[maxRank * maxFile];
            
            for (int i = 0; i < maxRank * maxFile; i++)
            {
                SelectionIndicator(i);
                switch (config[i])
                {
                    case PieceType.WPawn:
                        SpawnPiece(i, 0, 0, PieceType.WPawn, Color.White);
                        break;
                    case PieceType.WKnight:
                        SpawnPiece(i, 0, 1, PieceType.WKnight, Color.White);
                        break;
                    case PieceType.WBishop:
                        SpawnPiece(i, 0, 2, PieceType.WBishop, Color.White);
                        break;
                    case PieceType.Wrook:
                        SpawnPiece(i, 0, 3, PieceType.Wrook, Color.White);
                        break;
                    case PieceType.Wqueen:
                        SpawnPiece(i, 0, 4, PieceType.Wqueen, Color.White);
                        break;
                    case PieceType.Wking:
                        SpawnPiece(i, 0, 5, PieceType.Wking, Color.White);
                        break;
                    case PieceType.BPawn:
                        SpawnPiece(i, 1, 0, PieceType.BPawn, Color.Black);
                        break;
                    case PieceType.BKnight:
                        SpawnPiece(i, 1, 1, PieceType.BKnight, Color.Black);
                        break;
                    case PieceType.BBishop:
                        SpawnPiece(i, 1, 2, PieceType.BBishop, Color.Black);
                        break;
                    case PieceType.Brook:
                        SpawnPiece(i, 1, 3, PieceType.Brook, Color.Black);
                        break;
                    case PieceType.Bqueen:
                        SpawnPiece(i, 1, 4, PieceType.Bqueen, Color.Black);
                        break;
                    case PieceType.Bking:
                        SpawnPiece(i, 1, 5, PieceType.Bking, Color.Black);
                        break;
                    case PieceType.Nil:
                        pieces[i] = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SpawnPiece(int pos, int side, int typeAsFab, PieceType typeAsEnum, Color c)
        {
            var p = Instantiate(side == 0 ? whitePiecePrefabs[typeAsFab] : blackPiecePrefabs[typeAsFab]).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(pos / _maxRank, pos % _maxFile, typeAsEnum, c);
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

        public void Select(int pos)
        {
            selections[pos].gameObject.SetActive(true);
        }

        public void Unselect(int pos)
        {
            selections[pos].gameObject.SetActive(false);
        }
    }
}
