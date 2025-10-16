using Game.Common;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;
using UnityEngine.EventSystems;

namespace Game.Managers
{
    public enum Corner: byte
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

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
        public void Unselect(int pos)
        {
            selections[pos].GetComponent<MeshRenderer>().material = null;
            selections[pos].GetComponent<Marker>().enabled = false;
            selections[pos].gameObject.SetActive(false);
        }

        public void UnmarkAll()
        {
            for (var i = 0; i < BoardSize; i++)
            {
                selections[i].gameObject.SetActive(false);
            }
        }

        public void UnMark(int pos)
        {
            if (selections[pos]  != null)
            {
                selections[pos].gameObject.SetActive(false);
            }
        }
        public Corner IndexToCorner(Vector3 hit, Tile.Tile hoveringTile)
        {
            if (hit.x > hoveringTile.rank && hit.z < hoveringTile.file) return Corner.BottomLeft;
            if (hit.x > hoveringTile.rank && hit.z > hoveringTile.file) return Corner.BottomRight;
            if (hit.x < hoveringTile.rank && hit.z > hoveringTile.file) return Corner.TopLeft;
            if (hit.x < hoveringTile.rank && hit.z < hoveringTile.file) return Corner.TopRight;
            return Corner.TopLeft;
        }
        public void MarkTileInRange(Tile.Tile hoveringTile, int range, bool isMark, bool onlyMarkEnemy = false)
        {
            int centerIndex = IndexOf(hoveringTile.rank, hoveringTile.file);
            if (!IsActive(centerIndex)) return;
            if (range % 2 == 0) 
            {
                int startRank = hoveringTile.rank;
                int startFile = hoveringTile.file;
                if (hoveringTile.corner == Corner.BottomRight)
                {
                    startRank = startRank - range / 2 + 1;
                    startFile = startFile - range / 2 + 1;
                }
                else if (hoveringTile.corner == Corner.TopLeft)
                {
                    startFile = startFile - range / 2 + 1;
                    startRank = startRank - range / 2;
                }
                else if (hoveringTile.corner == Corner.TopRight)
                {
                    startRank = startRank - range / 2;
                    startFile = startFile - range / 2;
                }
                else if (hoveringTile.corner == Corner.BottomLeft)
                {
                    startRank = startRank - range / 2 + 1;
                    startFile = startFile - range / 2;
                }

                for (int r = startRank; r < startRank + range; r++)
                {
                    for (int f = startFile; f < startFile + range; f++)
                    {
                        int index = IndexOf(r, f);
                        if (!IsActive(index)) continue;
                        ApplyMarkingRule(index, isMark, onlyMarkEnemy);
                    }
                }
                return;
            } 


            int radius = range / 2;

            for (int r = hoveringTile.rank - radius; r <= hoveringTile.rank + radius; r++)
            {
                for (int f = hoveringTile.file - radius; f <= hoveringTile.file + radius; f++)
                {
                    int index = IndexOf(r, f);
                    if (!IsActive(index)) continue;

                    ApplyMarkingRule(index, isMark, onlyMarkEnemy);
                }
            }
        }

        /// <summary>
        /// Đánh dấu hoặc bỏ đánh dấu ô theo rule enemy / ally
        /// </summary>
        private void ApplyMarkingRule(int index, bool isMark, bool onlyMarkEnemy)
        {
            if (!isMark)
            {
                UnMark(index);
                return;
            }

            if (!onlyMarkEnemy)
            {
                MarkAsMoveable(index);
                return;
            }

            var piece = PieceOn(index);
            if (piece != null && piece.Color != MatchManager.Ins.GameState.OurSide)
            {
                MarkAsMoveable(index);
            }
        }


        public void MarkAsMoveable(int pos)
        {
            selections[pos].GetComponent<MeshRenderer>().material = moveableMat;
            selections[pos].GetComponent<Marker>().enabled = false;
            selections[pos].gameObject.SetActive(true);
        }

        public void MarkIfDifferntColor(bool color)
        {
            for (var i = 0; i < BoardSize; i++)
            {
                if (PieceOn(i) == null) continue;

                if (PieceOn(i).Color != color)
                {
                    selections[i].GetComponent<MeshRenderer>().material = moveableMat;
                    selections[i].GetComponent<Marker>().enabled = false;
                    selections[i].gameObject.SetActive(true);
                } else selections[i].gameObject.SetActive(false);
            }
        }
    }
}
