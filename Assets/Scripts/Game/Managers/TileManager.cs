using Game.Common;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;
using System;
namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TileManager : Singleton<TileManager>
    {
        private Tile.Tile[] tiles;
        [SerializeField] private Material moveableMat;
        [SerializeField] private Material selectionMat;

        private Marker[] selections;
        
        private void SelectionIndicator(int pos, Tile.Tile tile)
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

        public void Spawn()
        {
            selections = new Marker[BoardSize];
            tiles = new Tile.Tile[BoardSize];

            for (var i = 0; i < BoardSize; i++)
            {
                SpawnTile(i);
            }
        }

        private void SpawnTile(int index)
        {
            var prefab = IsActive(index)
                ? !ColorOfSquare(index) ? 
                    AssetManager.Ins.TileData[Color.White] : 
                    AssetManager.Ins.TileData[Color.Black] : 
                AssetManager.Ins.TileData[Color.None];

            var tile = Instantiate(prefab.gameObject, transform).GetComponent<Tile.Tile>();
            tile.Spawn(index);

            tiles[index] = tile;
            SelectionIndicator(index, tile);
        }

        public void DestroyTile(int index)
        {
            Destroy(tiles[index]);
            tiles[index] = null;
        }
        
        public void Select(int pos)
        {
            selections[pos].GetComponent<MeshRenderer>().material = selectionMat;
            selections[pos].GetComponent<Marker>().enabled = true;
            selections[pos].gameObject.SetActive(true);
        }

        public void UnmarkAll()
        {
            for (var i = 0; i < BoardSize; i++)
            {
                selections[i].gameObject.SetActive(false);
            }
        }

        public void MarkAsMoveable(int pos)
        {
            selections[pos].GetComponent<MeshRenderer>().material = moveableMat;
            selections[pos].GetComponent<Marker>().enabled = false;
            selections[pos].gameObject.SetActive(true);
        }
    }
}
