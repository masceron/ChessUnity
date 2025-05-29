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
        private bool[][] _boardMask;
        private Tiles _tiles;
    
        private GameObject[][] _piecePrefabs;
        
        private Pillars _pillars;
    
        [SerializeField] private TextAsset startpos;
    
        [SerializeField] private GameObject[] prefabsWhite;
        [SerializeField] private GameObject[] prefabsBlack;
        [SerializeField] private GameObject[] pillarPrefabs;

        private Pieces _pieces;

        private Vector2Int _selecting;

        [SerializeField] private GameObject cameraObject;

        private MyCamera _camera;

        private void Awake()
        {
            _selecting = -Vector2Int.one;
            _piecePrefabs = new GameObject[2][];
            _piecePrefabs[0] = prefabsWhite;
            _piecePrefabs[1] = prefabsBlack;
            _camera = cameraObject.GetComponent<MyCamera>();

            GenerateTiles(1.5f);
            SpawnAllPieces();
        }
    
        private void GenerateTiles(float tileSize)
        {
            const int offset = (MaxTileNum - TileNum) / 2;
            const int end = MaxTileNum - offset - 1;
            
            _boardMask = new bool[MaxTileNum][];

            _pillars = new GameObject("Pillars").AddComponent<Pillars>();
            _pillars.transform.parent = transform;
            _pillars.PillarsComponents = new Pillar[MaxTileNum][];
            _pillars.pillarPrefabs = pillarPrefabs;
        
            _tiles = new GameObject("Tiles").AddComponent<Tiles>();
            _tiles.transform.parent = transform;
            _tiles.maxTileNum = MaxTileNum;
            _tiles.BoardMask = _boardMask;
            _tiles.BoardTiles = new Tile[MaxTileNum][];
            _tiles.tileMat = tileMat;

            for (var i = 0; i < MaxTileNum; i++)
            {
                _boardMask[i] = new bool[MaxTileNum];
                _tiles.BoardTiles[i] = new Tile[MaxTileNum];
                _pillars.PillarsComponents[i] = new Pillar[MaxTileNum];

                for (var j = 0; j < MaxTileNum; j++)
                {
                    if (i is >= offset and <= end && j is >= offset and <= end)
                    {
                        _tiles.GenerateTile(tileSize, i, j, true);
                        _pillars.CreatePillar(tileSize, i, j, true);
                        _boardMask[i][j] = true;
                    }
                    else
                    {
                        _tiles.GenerateTile(tileSize, i, j, false);
                        _boardMask[i][j] = false;
                        _pillars.CreatePillar(tileSize, i, j, false);
                    }
                }
            }

        }

        private void SelectMove(int x, int y)
        {
            var selected = new Vector2Int(x, y);

            if (_selecting == -Vector2Int.one)
            {
                if (!_pieces.PiecesArr[x][y]) return;
                _selecting = selected;
                _tiles.BoardTiles[_selecting.x][_selecting.y].Select(false);
                _pieces.PiecesArr[_selecting.x][_selecting.y].Select();
            }
            else
            {
                if (selected == _selecting)
                {
                    _pieces.PiecesArr[_selecting.x][_selecting.y].Unselect();
                    _tiles.BoardTiles[_selecting.x][_selecting.y].Unselect(false);
                    _selecting = -Vector2Int.one;
                    return;
                }

                var fx = _selecting.x;
                var fy = _selecting.y;
                var pieceOnSrc = _pieces.PiecesArr[fx][fy];
                var pieceOnDest = _pieces.PiecesArr[x][y];
                _tiles.BoardTiles[fx][fy].Unselect(false);
                _pieces.PiecesArr[fx][fy].Unselect();
                _selecting = -Vector2Int.one;
            
                if (pieceOnDest)
                {
                    if (pieceOnSrc.side == pieceOnDest.side)
                    {
                        _tiles.BoardTiles[x][y].Select(false);
                        _pieces.PiecesArr[x][y].Select();
                        _selecting = new Vector2Int(x, y);
                        return;
                    }
                }
                Move(pieceOnSrc, pieceOnDest, fx, fy, x, y);
            
            }
        }

        public void Select(int x, int y)
        {
            if (!_choosingExpansion)
            {
                SelectMove(x, y);
            }
            else
            {
                SelectExpansion(x, y);
            }
            
        }

        private void Move(Piece pieceOnSrc, Piece pieceOnDest, int fx, int fy, int x, int y)
        {
            _pieces.PiecesArr[fx][fy] = null;
        
            if (pieceOnDest)
            {
                Destroy(pieceOnDest.gameObject);
            }
            _pieces.PiecesArr[x][y] = pieceOnSrc;
        
            pieceOnSrc.Move(x, y);
        
        }

        private void SpawnAllPieces()
        {
            _pieces = new GameObject("Pieces").AddComponent<Pieces>();
            _pieces.PiecesArr = new Piece[MaxTileNum][];
            _pieces.PiecePrefabs = _piecePrefabs;
            
            for (var i = 0; i < MaxTileNum; i++)
            {
                _pieces.PiecesArr[i] = new Piece[MaxTileNum];
                for (var j = 0; j < MaxTileNum; j++)
                {
                    _pieces.PiecesArr[i][j] = null;
                }
            }

            var pieces = JsonUtility.FromJson<PiecesData>(startpos.text);
        
            foreach (var piece in pieces.pieces)
            {
                _pieces.SpawnPiece((Side)piece.side, (PieceType)piece.piece, piece.pos[0], piece.pos[1]);
            }
        }

        private void Activate(int x, int y)
        {
            _tiles.BoardTiles[x][y].Unselect(false);
            
            _pillars.Activate(x, y);
            _camera.Shake();
        }

        public void ActivateTile(int x, int y)
        {
            _tiles.Activate(x, y);
            _boardMask[x][y] = true;
        }

        public void Deactivate(int x, int y)
        {
            _tiles.Deactivate(x, y);
            _pillars.Deactivate(x, y);
            _boardMask[x][y] = false;
            _camera.Shake();
        }

        private bool _choosingExpansion;
        private List<Vector2Int> _expandingTiles;
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
            
            if (!_choosingExpansion)
            {
                _expandingTiles = new List<Vector2Int>();
                _choosingExpansion = true;
                _tiles.ExpansionStart();
                return;
            }

            if (_expandingTiles.Count > 0)
            {
                foreach (var pos in _expandingTiles)
                {
                    Activate(pos.x, pos.y);
                }
            }
            _choosingExpansion = false;
            _expandingTiles.Clear();
            _tiles.ExpansionEnd();
        }

        private void SelectExpansion(int x, int y)
        {
            var selected = new Vector2Int(x, y);
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
