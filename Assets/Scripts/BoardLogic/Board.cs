using System.Collections.Generic;
using Common;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace BoardLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        private const int TileNum = 8;
        private const int MaxTileNum = 12;
    
        [SerializeField] private Material tileMat;
        private bool[] _boardMask;
        private Tiles _tiles;
    
        private GameObject[][] _piecePrefabs;
        
        private Pillars _pillars;
    
        [SerializeField] private TextAsset startpos;
    
        [SerializeField] private GameObject[] prefabsWhite;
        [SerializeField] private GameObject[] prefabsBlack;
        [SerializeField] private GameObject[] pillarPrefabs;

        private Pieces _pieces;

        private int _selecting;

        [SerializeField] private GameObject cameraObject;

        private MyCamera _camera;

        private void Awake()
        {
            _selecting = -1;
            _camera = cameraObject.GetComponent<MyCamera>();

            GenerateTiles(1.5f);
            SpawnAllPieces();
        }
    
        private void GenerateTiles(float tileSize)
        {
            const int offset = (MaxTileNum - TileNum) / 2;
            const int end = MaxTileNum - offset - 1;
            
            _boardMask = new bool[MaxTileNum * MaxTileNum];

            _pillars = new GameObject("Pillars").AddComponent<Pillars>();
            _pillars.transform.parent = transform;
            _pillars.pillarsComponents = new Pillar[MaxTileNum * MaxTileNum];
            _pillars.pillarPrefabs = pillarPrefabs;
            _pillars.maxTileNum = MaxTileNum;
        
            _tiles = new GameObject("Tiles").AddComponent<Tiles>();
            _tiles.transform.parent = transform;
            _tiles.maxTileNum = MaxTileNum;
            _tiles.boardMask = _boardMask;
            _tiles.boardTiles = new Tile[MaxTileNum * MaxTileNum];
            _tiles.tileMat = tileMat;

            for (var i = 0; i < MaxTileNum; i++)
            {
                for (var j = 0; j < MaxTileNum; j++)
                {
                    if (i is >= offset and <= end && j is >= offset and <= end)
                    {
                        _tiles.GenerateTile(tileSize, i, j, true);
                        _pillars.CreatePillar(tileSize, i, j, true);
                        _boardMask[i * MaxTileNum + j] = true;
                    }
                    else
                    {
                        _tiles.GenerateTile(tileSize, i, j, false);
                        _boardMask[i * MaxTileNum + j] = false;
                        _pillars.CreatePillar(tileSize, i, j, false);
                    }
                }
            }

        }

        private void SelectMove(int selected)
        {
            if (_selecting == -1)
            {
                if (!_pieces.piecesArr[selected]) return;
                _selecting = selected;
                _tiles.boardTiles[_selecting].Select(false);
                _pieces.piecesArr[_selecting].Select();
            }
            else
            {
                if (selected == _selecting)
                {
                    _pieces.piecesArr[_selecting].Unselect();
                    _tiles.boardTiles[_selecting].Unselect(false);
                    _selecting = -1;
                    return;
                }

                var old = _selecting;
                var pieceOnSrc = _pieces.piecesArr[_selecting];
                var pieceOnDest = _pieces.piecesArr[selected];
                _tiles.boardTiles[_selecting].Unselect(false);
                _pieces.piecesArr[_selecting].Unselect();
                _selecting = -1;
            
                if (pieceOnDest)
                {
                    if (pieceOnSrc.side == pieceOnDest.side)
                    {
                        _tiles.boardTiles[selected].Select(false);
                        _pieces.piecesArr[selected].Select();
                        _selecting = selected;
                        return;
                    }
                }
                Move(pieceOnSrc, pieceOnDest, old, selected);
            
            }
        }

        public void Select(int row, int col)
        {
            var index = row * MaxTileNum + col;
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
            _pieces.piecesArr[from] = null;
        
            if (pieceOnDest)
            {
                Destroy(pieceOnDest.gameObject);
            }
            _pieces.piecesArr[to] = pieceOnSrc;
            
            pieceOnSrc.Move(to / MaxTileNum, to % MaxTileNum);
        
        }

        private void SpawnAllPieces()
        {
            _piecePrefabs = new GameObject[2][];
            _piecePrefabs[0] = prefabsWhite;
            _piecePrefabs[1] = prefabsBlack;
            
            _pieces = new GameObject("Pieces").AddComponent<Pieces>();
            _pieces.piecesArr = new Piece[MaxTileNum * MaxTileNum];
            _pieces.PiecePrefabs = _piecePrefabs;
            _pieces.maxTileNum = MaxTileNum;
            
            for (var i = 0; i < MaxTileNum; i++)
            {
                for (var j = 0; j < MaxTileNum; j++)
                {
                    _pieces.piecesArr[i * MaxTileNum + j] = null;
                }
            }

            var pieces = JsonUtility.FromJson<PiecesData>(startpos.text);
        
            foreach (var piece in pieces.pieces)
            {
                _pieces.SpawnPiece((Side)piece.side, (PieceType)piece.piece, piece.pos[0], piece.pos[1]);
            }
        }

        private void Activate(int index)
        {
            _tiles.boardTiles[index].Unselect(false);
            
            _pillars.Activate(index);
            _camera.Shake();
        }

        public void ActivateTile(int index)
        {
            _tiles.Activate(index);
            _boardMask[index] = true;
        }

        public void Deactivate(int row, int col)
        {
            var index = row * MaxTileNum + col;
            
            _tiles.Deactivate(index);
            _pillars.Deactivate(index);
            _boardMask[index] = false;
            _camera.Shake();
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
            _expandingTiles.Clear();
            _tiles.ExpansionEnd();
        }

        private void SelectExpansion(int selected)
        {
            if (!_expandingTiles.Contains(selected))
            {
                if (_expandingTiles.Count >= 3) return;
                _expandingTiles.Add(selected);
                _tiles.SelectExpand(selected);
            }
            else
            {
                _expandingTiles.Remove(selected);
                _tiles.UnSelectExpand(selected);
            }
        }
    }
}
