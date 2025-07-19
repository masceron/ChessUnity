using System.Collections;
using Game.Board.Interaction;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Board.Tile
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TileManager : MonoBehaviour
    {
        private Tile[] tiles;
        
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
            sel.transform.position = new Vector3(RankOf(pos), 1.15f, FileOf(pos));
            sel.name = "Selection " + pos;
            
            sel.SetActive(false);
            
            selections[pos] = sel.AddComponent<Marker>();
        }

        public void Spawn(BitArray active)
        {
            selections = new Marker[BoardSize];
            tiles = new Tile[BoardSize];
            
            for (var i = 0; i < MaxLength; i++)
            {
                var rankStart = RowIndex(i);
                for (var j = 0; j < MaxLength; j++)
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
