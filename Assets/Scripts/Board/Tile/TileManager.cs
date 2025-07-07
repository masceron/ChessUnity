using System.Collections;
using Board.Interaction;
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
        [SerializeField] private Material moveableMat;
        [SerializeField] private Material selectionMat;

        private Marker[] selections;
        
        private void SelectionIndicator(int pos, Tile tile)
        {
            var sel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sel.transform.localScale = new Vector3(1, 0.001f, 1);
            sel.transform.parent = tile.transform;
            sel.layer = LayerMask.NameToLayer("Ignore Raycast");
            sel.transform.position = new Vector3(pos / _maxRank, 1.15f, pos % _maxFile);
            sel.name = "Selection " + pos;
            
            sel.SetActive(false);
            
            selections[pos] = sel.AddComponent<Marker>();
        }

        public void Spawn(int maxRank, int maxFile, BitArray active)
        {
            _maxRank = maxRank;
            _maxFile = maxFile;
            
            selections = new Marker[maxRank * maxFile];
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
                    SelectionIndicator(index, tile);
                }
            }
        }
        
        public void Select(int pos)
        {
            selections[pos].GetComponent<MeshRenderer>().material = selectionMat;
            selections[pos].GetComponent<Marker>().enabled = true;
            selections[pos].gameObject.SetActive(true);
        }

        public void Unmark(int pos)
        {
            selections[pos].gameObject.SetActive(false);
        }

        public void MarkAsMoveable(int pos)
        {
            selections[pos].GetComponent<MeshRenderer>().material = moveableMat;
            selections[pos].GetComponent<Marker>().enabled = false;
            selections[pos].gameObject.SetActive(true);
        }
    }
}
