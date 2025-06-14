using UnityEngine;

namespace BoardLogic
{
    public class Tiles : MonoBehaviour
    {
        private Tile[] _boardTiles;
        private int _maxTileNumX;
        private int _maxTileNumY;
        [SerializeField] private Material tileMat;
        private bool[] _boardMask;
        private float _tileSize;

        public void Init()
        {
            _tileSize = transform.parent.GetComponent<Board>().TileSize;
            _maxTileNumX = transform.parent.GetComponent<Board>().MaxTileNumX;
            _maxTileNumY = transform.parent.GetComponent<Board>().MaxTileNumY;
            
            _boardTiles = new Tile[_maxTileNumX * _maxTileNumY];
            _boardMask = new bool[_maxTileNumX * _maxTileNumY];
        }

        public void GenerateTile(int row, int col, bool active)
        {
            var tile = new GameObject($"Tile {row}, {col}");

            var mesh = new Mesh();
            var script = tile.AddComponent<Tile>();
            script.transform.parent = transform;
            script.Set(row, col);

            tile.AddComponent<MeshFilter>().mesh = mesh;
            tile.AddComponent<MeshRenderer>().material = tileMat;

            var vertices = new Vector3[4];
            vertices[0] = new Vector3(row * _tileSize, 0, col * _tileSize);
            vertices[1] = new Vector3(row * _tileSize, 0, (col + 1) * _tileSize);
            vertices[2] = new Vector3((row + 1) * _tileSize, 0, col * _tileSize);
            vertices[3] = new Vector3((row + 1) * _tileSize, 0, (col + 1) * _tileSize);

            var triangles = new[] { 0, 1, 2, 1, 3, 2 };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            tile.AddComponent<BoxCollider>();
            tile.GetComponent<BoxCollider>().isTrigger = true;
            tile.layer = LayerMask.NameToLayer(active ? "Tile" : "Ignore Raycast");
            
            _boardMask[row * _maxTileNumY + col] = active;
            _boardTiles[row * _maxTileNumY + col] = script;
        }
        public void Activate(int index)
         {
             _boardTiles[index].Activate();
             _boardMask[index] = true;
         }

        public void Deactivate(int index)
        {
            _boardTiles[index].Deactivate();
            _boardMask[index] = false;
        }

        public void ExpansionStart()
        {
            for (var i = 0; i < _maxTileNumX; i++)
            {
                var row = i * _maxTileNumY;
                for (var j = 0; j < _maxTileNumY; j++)
                {
                    var index = row + j;
                    if (!_boardMask[index])
                    {
                        _boardTiles[index].MarkAsExpandable();
                        _boardTiles[index].gameObject.layer = LayerMask.NameToLayer("Tile");
                    }
                    else
                    {
                        _boardTiles[index].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }
        }

        public void ExpansionEnd()
        {
            for (var i = 0; i < _maxTileNumX; i++)
            {
                var row = i * _maxTileNumY;
                for (var j = 0; j < _maxTileNumY; j++)
                {
                    var index = row + j;
                    if (!_boardMask[index])
                    {
                        _boardTiles[index].UnmarkAsExpandable();
                        _boardTiles[index].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                    else
                    {
                        _boardTiles[index].gameObject.layer = LayerMask.NameToLayer("Tile");
                    }
                }
            }
        }

        public void Select(int select, bool expand)
        {
            _boardTiles[select].Select(expand);
        }

        public void UnSelect(int select, bool expand)
        {
            _boardTiles[select].Unselect(expand);
        }
        
        public void Block(int index)
        {
            _boardTiles[index].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void Unblock(int index)
        {
            _boardTiles[index].gameObject.layer = LayerMask.NameToLayer("Tile");
        }
    }
    
    
}
