using System;
using Game.Board.PieceLogic;
using Resources.ScriptableObjects.Pieces;
using UnityEngine;
using Color = Game.Board.General.Color;

namespace Game.Board.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceManager : MonoBehaviour
    {
        private Piece[] pieces;
        
        private PieceObject[] pieceObjects;
        
        private int maxFile;
        private int maxRank;
    
        public void Spawn(int r, int f, PieceObject[] objects,  PieceLogic.PieceLogic[] config)
        {
            maxFile = r;
            maxRank = f;
            pieceObjects = objects;
            pieces = new Piece[maxRank * maxFile];
            
            for (var i = 0; i < maxRank * maxFile; i++)
            {
                if (config[i] == null) continue;
                switch (config[i])
                {
                    case Velkaris:
                        SpawnPiece(i, 0, config[i].Color);
                        break;
                    case GuidingSiren:
                        SpawnPiece(i, 1, config[i].Color);
                        break;
                    case Barracuda:
                        SpawnPiece(i, 2, config[i].Color);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SpawnPiece(int pos, int typeAsFab, Color color)
        {
            var prefab = pieceObjects[typeAsFab].prefab;
            var p = Instantiate(prefab).AddComponent<Piece>();
            p.transform.parent = transform;
            pieces[pos] = p;
            p.Spawn(pos / maxRank, pos % maxFile, color, pieceObjects[typeAsFab].defaultTransform);
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
