using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Board.Tile
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TileManager : MonoBehaviour
    {
        private Tile[] tiles;
        private int _maxRank;
        private int _maxFile;
        
        [SerializeField] private GameObject[] floorPrefabs;

        public void Spawn(int maxRank, int maxFile, bool[] active)
        {
            _maxRank = maxRank;
            _maxFile = maxFile;
            
            tiles = new Tile[_maxRank * _maxFile];
            
            for (var i = 0; i < _maxRank; i++)
            {
                var rankStart = i * maxFile;
                for (var j = 0; j < _maxFile; j++)
                {
                    var index = rankStart + j;
                    var tile = new GameObject(i + " " + j).AddComponent<Tile>();
                    tile.transform.parent = transform;
                    if (active[index])
                        tile.Spawn(i, j, floorPrefabs[(i + j) % 2], true);
                    else tile.Spawn(i, j, floorPrefabs[2], false);

                    tiles[index] = tile;
                }
            }
        }
    }
}
