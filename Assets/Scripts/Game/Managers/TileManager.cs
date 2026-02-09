using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;

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
        [SerializeField] private ActiveBoardBorder boardBorder3D;

        /*[SerializeField] private Button _activeTileButton;
        public int rank, file;*/

        private Marker[] selections;

        /*private void Start()
        {
            _activeTileButton.onClick.AddListener(() => DestroyTile(rank, file));
        }*/

        public Tile.Tile GetTile(int pos)
        {
            if (!VerifyIndex(pos)) return null;
            return tiles[pos];
        }

        public int GetTileValue(int pos)
        {
            if (!VerifyIndex(pos)) return -1000;
            return tiles[pos].GetTileValue();
        }

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

            UpdateBorder();
            SetActiveTiles();
        }

        public void SetActiveTiles()
        {
            if (tiles == null || tiles.Length == 0 || ActiveBoard() == null) return;

            for (var y = 0; y < MaxLength; y++)
            {
                for (var x = 0; x < MaxLength; x++)
                {
                    var index = IndexOf(x, y);
                    if (tiles[index] == null) continue;

                    tiles[index].gameObject.SetActive(ShouldTileBeActive(x, y));
                }
            }
        }


        /// <summary>
        /// Cập nhật vùng 3x3 quanh ô (x, y) sau khi active hoặc destroy.
        /// </summary>
        private void UpdateActiveRegion(int x, int y)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    var nx = x + dx;
                    var ny = y + dy;

                    if (!VerifyBounds(nx) || !VerifyBounds(ny))
                        continue;

                    var index = IndexOf(nx, ny);
                    if (tiles[index] == null) continue;

                    tiles[index].gameObject.SetActive(ShouldTileBeActive(nx, ny));
                }
            }
        }


        /// <summary>
        /// Kiểm tra 1 ô có nên được active (hiển thị) hay không.
        /// </summary>
        private bool ShouldTileBeActive(int x, int y)
        {
            var index = IndexOf(x, y);
            if (tiles[index] == null) return false;

            var isActive = IsActive(index);
            if (isActive) return true;

            // Nếu ô này chưa active, kiểm tra 8 ô xung quanh
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var nx = x + dx;
                    var ny = y + dy;

                    if (!VerifyBounds(nx) || !VerifyBounds(ny))
                        continue;

                    var neighborIndex = IndexOf(nx, ny);
                    if (IsActive(neighborIndex))
                        return true;
                }
            }

            return false;
        }


        public void ActivateTile(int x, int y)
        {
            var index = IndexOf(x, y);
            if (IsActive(index)) return;

            SetActiveSquare(index, true);

            // Nếu tile none thì đổi sang white/black
            if (tiles[index] != null && tiles[index].color == Color.None)
            {
                var prefab = !ColorOfSquare(index)
                    ? AssetManager.Ins.tileData[Color.White]
                    : AssetManager.Ins.tileData[Color.Black];

                Destroy(tiles[index].gameObject);
                var tile = Instantiate(prefab.gameObject, transform).GetComponent<Tile.Tile>();
                tile.Spawn(index);
                tiles[index] = tile;
            }

            // Cập nhật vùng hiển thị quanh ô
            UpdateActiveRegion(x, y);
            UpdateBorder();
        }

        public void UpdateBorder()
        {
            if (boardBorder3D == null) boardBorder3D = FindFirstObjectByType<ActiveBoardBorder>();
            boardBorder3D.UpdateBorder();
        }

        private void SpawnTile(int index)
        {
            var prefab = IsActive(index)
                ? !ColorOfSquare(index) ? 
                    AssetManager.Ins.tileData[Color.White] : 
                    AssetManager.Ins.tileData[Color.Black] : 
                AssetManager.Ins.tileData[Color.None];

            var tile = Instantiate(prefab.gameObject, transform).GetComponent<Tile.Tile>();
            tile.Spawn(index);

            tiles[index] = tile;
            SelectionIndicator(index, tile);
        }

        public void DestroyTile(int rank, int file)
        {
            DestroyTile(IndexOf(rank, file));
        }

        public void DestroyTile(int index)
        {
            Destroy(tiles[index].gameObject);
            tiles[index] = null;

            var prefab = AssetManager.Ins.tileData[Color.None];
            var tile = Instantiate(prefab.gameObject, transform).GetComponent<Tile.Tile>();
            tile.Spawn(index);

            tiles[index] = tile;
            SelectionIndicator(index, tile);
            SetActiveSquare(index, false);

            var x = RankOf(index);
            var y = FileOf(index);
            UpdateActiveRegion(x, y);
            UpdateBorder();
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
            var centerIndex = IndexOf(hoveringTile.rank, hoveringTile.file);
            if (!IsActive(centerIndex)) return;
            if (range % 2 == 0) 
            {
                var startRank = hoveringTile.rank;
                var startFile = hoveringTile.file;
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

                for (var r = startRank; r < startRank + range; r++)
                {
                    for (var f = startFile; f < startFile + range; f++)
                    {
                        var index = IndexOf(r, f);
                        if (!IsActive(index)) continue;
                        ApplyMarkingRule(index, isMark, onlyMarkEnemy);
                    }
                }
                return;
            } 


            var radius = range / 2;

            for (var r = hoveringTile.rank - radius; r <= hoveringTile.rank + radius; r++)
            {
                for (var f = hoveringTile.file - radius; f <= hoveringTile.file + radius; f++)
                {
                    var index = IndexOf(r, f);
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

        public void MarkNextEachPiece(bool color, int pos)
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var indexOff = IndexOf(RankOf(pos) + i, FileOf(pos) + j);

                    if (!VerifyIndex(indexOff)) continue;
                    var pieceOff = PieceOn(indexOff);
                    if (pieceOff == null) continue;
                    if (pieceOff.Color != color) continue;
                    MarkAsMoveable(indexOff);
                }
            }
        }

        public bool IsTileEmpty(int index)
        {
            return tiles[index] == null || tiles[index].color == Color.None;
        }

        public void MarkPieceInRange(int pos, bool color, int range)
        {
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(pos), FileOf(pos), range))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                if (pOn == null) continue;

                if (pOn.Color == color) MarkAsMoveable(idx);
            }
        }
    }
}
