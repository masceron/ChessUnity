using System.Collections.Generic;
using Common;
using Core;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace BoardLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        private const int TileNum = 8;
        public int maxTileNum = 12;
        
        private Tiles _tiles;
        private Pillars _pillars;
        private Pieces _pieces;
    
        [SerializeField] private TextAsset startpos;

        public readonly float TileSize = 1.5f;

        private int _selecting;

        [SerializeField] private GameObject cameraObject;

        private MyCamera _camera;

        private void Awake()
        {
            _selecting = -1;
            _camera = cameraObject.GetComponent<MyCamera>();

            MakeBlockers();
            GenerateTiles();
            SpawnAllPieces();
            SetupEngine();
        }
    
        private void GenerateTiles()
        {
            var offset = (maxTileNum - TileNum) / 2;
            var end = maxTileNum - offset - 1;

            _pillars = transform.Find("Pillars").GetComponent<Pillars>();
            _pillars.transform.parent = transform;
            _pillars.Init();
        
            _tiles = transform.Find("Tiles").GetComponent<Tiles>();
            _tiles.transform.parent = transform;
            _tiles.Init();

            for (var i = 0; i < maxTileNum; i++)
            {
                for (var j = 0; j < maxTileNum; j++)
                {
                    if (i >= offset && i <= end && j >= offset && j <= end)
                    {
                        _tiles.GenerateTile(i, j, true);
                        _pillars.CreatePillar(i, j, true);
                    }
                    else
                    {
                        _tiles.GenerateTile(i, j, false);
                        _pillars.CreatePillar(i, j, false);
                    }
                }
            }

        }

        private void SelectMove(int selected)
        {
            if (_selecting == -1)
            {
                if (!_pieces.Get(selected)) return;
                _selecting = selected;
                _tiles.Select(_selecting, false);
                _pieces.Select(_selecting);
            }
            else
            {
                if (selected == _selecting)
                {
                    _pieces.Unselect(_selecting);
                    _tiles.UnSelect(_selecting, false);
                    _selecting = -1;
                    return;
                }

                var old = _selecting;
                var pieceOnSrc = _pieces.Get(_selecting);
                var pieceOnDest = _pieces.Get(selected);
                _tiles.UnSelect(_selecting, false);
                _pieces.Unselect(_selecting);
                _selecting = -1;
            
                if (pieceOnDest)
                {
                    if (pieceOnSrc.Side() == pieceOnDest.Side())
                    {
                        _tiles.Select(_selecting, false);
                        _pieces.Select(selected);
                        _selecting = selected;
                        return;
                    }
                }
                Move(pieceOnSrc, pieceOnDest, old, selected);
            
            }
        }

        public void Select(int row, int col)
        {
            var index = row * maxTileNum + col;
            if (!_choosingExpansion)
            {
                SelectMove(index);
            }
            else
            {
                SelectExpansion(index);
            }
            
        }

        private void Move(Piece pieceOnSrc, Piece pieceOnDest, int from, int to)
        {
            _pieces.Set(from, null);
        
            if (pieceOnDest)
            {
                Destroy(pieceOnDest.gameObject);
            }
            _pieces.Set(to, pieceOnSrc);
            
            pieceOnSrc.Move(to / maxTileNum, to % maxTileNum);
        
        }

        private void SpawnAllPieces()
        {
            _pieces = transform.Find("Pieces").GetComponent<Pieces>();
            _pieces.Init();
            
            for (var i = 0; i < maxTileNum; i++)
            {
                for (var j = 0; j < maxTileNum; j++)
                {
                    _pieces.Set(i * maxTileNum + j, null);
                }
            }

            var pieces = JsonUtility.FromJson<PiecesData>(startpos.text);
        
            foreach (var piece in pieces.pieces)
            {
                _pieces.SpawnPiece((PieceSide)piece.side, (PieceType)piece.piece, piece.pos[0], piece.pos[1]);
            }
        }

        private void Activate(int index)
        {
            _tiles.Select(index, false);
            
            _pillars.Activate(index);
            _camera.Shake();
        }

        public void ActivateTile(int index)
        {
            _tiles.Activate(index);
        }

        public void Deactivate(int row, int col)
        {
            var index = row * maxTileNum + col;
            
            _tiles.Deactivate(index);
            _pillars.Deactivate(index);
            _camera.Shake();
        }


        private Blockers _blockers;

        private void MakeBlockers()
        {
            _blockers = transform.Find("Blockers").GetComponent<Blockers>();
            _blockers.Init();
        }
        public void Block(int row, int col)
        {
            var index = row * maxTileNum + col;
            _tiles.Block(index);
            _blockers.Block(row, col, index);
        }

        public void Unblock(int row, int col)
        {
            var index = row * maxTileNum + col;
            _tiles.Unblock(index);
            _blockers.Unblock(index);
        }

        private bool _choosingExpansion;
        private List<int> _expandingTiles;
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
            
            if (!_choosingExpansion)
            {
                _expandingTiles = new List<int>();
                _choosingExpansion = true;
                _tiles.ExpansionStart();
                return;
            }

            if (_expandingTiles.Count > 0)
            {
                foreach (var pos in _expandingTiles)
                {
                    Activate(pos);
                }
            }
            _choosingExpansion = false;
            _tiles.ExpansionEnd();
        }

        private void SelectExpansion(int selected)
        {
            if (!_expandingTiles.Contains(selected))
            {
                if (_expandingTiles.Count >= 3) return;
                _expandingTiles.Add(selected);
                _tiles.Select(selected, true);
            }
            else
            {
                _expandingTiles.Remove(selected);
                _tiles.UnSelect(selected, true);
            }
        }

        private Internal _internal;

        private void SetupEngine()
        {
            _internal = new Internal();
        }
    }
}
