using UnityEngine;
using System.Collections;
using Game.Managers;
using Game.Common;
using System.Collections.Generic;
using System.Linq;

namespace Game.Tile
{
    public class ActiveBoardBorder : MonoBehaviour
    {
        [Header("Board Settings")]
        public int boardWidth => BoardUtils.MaxLength;
        public int boardHeight => BoardUtils.MaxLength;
        public BitArray ActiveBoard => MatchManager.Ins.GameState.ActiveBoard;

        public float heightOffset = 1.2f;

        [Header("Border Settings")]
        public float lineWidth = 0.1f;
        public UnityEngine.Color lineColor = UnityEngine.Color.blue;
        public bool tightBorder = false; // use Minmax or Tight Outer Border

        [Header("Middle Line Settings")]
        public UnityEngine.Color middleLineColor = UnityEngine.Color.red;
        public float middleLineWidth = 0.05f;

        [SerializeField] private LineRenderer borderRenderer;
        [SerializeField] private LineRenderer midLineRenderer;

        private int minX, maxX, minY, maxY;
        private float borderZMin, borderZMax;

        /*public int rank;
        public int file;
        public UnityEngine.UI.Button button;*/
        void Awake()
        {
            SetupLine(borderRenderer, lineWidth, lineColor, true);
            SetupLine(midLineRenderer, middleLineWidth, middleLineColor, false);
            /*button.onClick.AddListener(() =>
            {
                ActiveBoard[IndexOf(rank, file)] = true;
                UpdateBorder();
            });*/
        }

        void SetupLine(LineRenderer lr, float width, UnityEngine.Color color, bool loop)
        {
            lr.useWorldSpace = true;
            lr.loop = loop;
            lr.startWidth = width;
            lr.endWidth = width;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = color;
            lr.endColor = color;
        }

        public void UpdateBorder()
        {
            if (ActiveBoard == null || ActiveBoard.Length == 0)
            {
                borderRenderer.positionCount = 0;
                midLineRenderer.positionCount = 0;
                return;
            }

            minX = boardWidth; maxX = -1; minY = boardHeight; maxY = -1;
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    int index = IndexOf(x, y);
                    if (ActiveBoard[index])
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            if (maxX == -1 || maxY == -1)
            {
                borderRenderer.positionCount = 0;
                midLineRenderer.positionCount = 0;
                return;
            }

            if (!tightBorder)
            {
                DrawMinMaxBorder();
            }
            else
            {
                DrawTightOuterBorder();
            }

            DrawMiddleLine();
        }

        private void DrawMinMaxBorder()
        {
            minX = boardWidth; maxX = -1; minY = boardHeight; maxY = -1;

            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    int index = IndexOf(x, y);
                    if (ActiveBoard[index])
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            if (maxX == -1 || maxY == -1)
            {
                borderRenderer.positionCount = 0;
                midLineRenderer.positionCount = 0;
                return;
            }

            float half = 0.5f;
            Vector3[] corners = new Vector3[]
            {
                new Vector3(minX - half, heightOffset, minY - half),
                new Vector3(minX - half, heightOffset, maxY + 1 - half),
                new Vector3(maxX + 1 - half, heightOffset, maxY + 1 - half),
                new Vector3(maxX + 1 - half, heightOffset, minY - half)
            };

            borderRenderer.loop = true;
            borderRenderer.positionCount = corners.Length;
            borderRenderer.SetPositions(corners);

            borderZMin = corners.Min(c => c.z);
            borderZMax = corners.Max(c => c.z);

        }
        private void DrawTightOuterBorder()
        {
            // We'll represent corner coordinates as integers doubled (avoid float halves).
            // corner for tile (x,y):
            // bl = (2*x-1, 2*y-1), tl = (2*x-1, 2*y+1), tr = (2*x+1, 2*y+1), br = (2*x+1, 2*y-1)

            // store remaining boundary edges: key = "minx_miny_maxx_maxy" (undirected),
            // value = the original directed start vertex (so we can reconstruct adjacency).
            var edges = new Dictionary<string, Vector2Int>();

            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    if (!IsActive(x, y)) continue;

                    Vector2Int bl = new Vector2Int(2 * x - 1, 2 * y - 1);
                    Vector2Int tl = new Vector2Int(2 * x - 1, 2 * y + 1);
                    Vector2Int tr = new Vector2Int(2 * x + 1, 2 * y + 1);
                    Vector2Int br = new Vector2Int(2 * x + 1, 2 * y - 1);

                    TryAddEdge(edges, tl, bl); // left edge (top -> bottom)
                    TryAddEdge(edges, bl, br); // bottom (left -> right)
                    TryAddEdge(edges, br, tr); // right (bottom -> top)
                    TryAddEdge(edges, tr, tl); // top (right -> left)
                }
            }

            if (edges.Count == 0)
            {
                borderRenderer.positionCount = 0;
                return;
            }

            var adj = new Dictionary<Vector2Int, List<Vector2Int>>();
            foreach (var kv in edges)
            {
                string key = kv.Key;
                Vector2Int start = kv.Value;
                var parts = key.Split('_').Select(int.Parse).ToArray();
                Vector2Int a = new Vector2Int(parts[0], parts[1]);
                Vector2Int b = new Vector2Int(parts[2], parts[3]);

                Vector2Int other = (start == a) ? b : a;

                if (!adj.ContainsKey(start)) adj[start] = new List<Vector2Int>();
                if (!adj.ContainsKey(other)) adj[other] = new List<Vector2Int>();
                adj[start].Add(other);
                adj[other].Add(start);
            }

            var loops = new List<List<Vector2Int>>();
            var visitedVertices = new HashSet<Vector2Int>();

            foreach (var kv in adj)
            {
                var v = kv.Key;
                if (visitedVertices.Contains(v)) continue;

                var loop = new List<Vector2Int>();
                Vector2Int startV = v;
                Vector2Int prev = new Vector2Int(int.MinValue, int.MinValue); // null
                Vector2Int curr = startV;

                while (true)
                {
                    loop.Add(curr);
                    visitedVertices.Add(curr);

                    var neighbors = adj[curr];
                    Vector2Int next;
                    if (neighbors.Count == 0) break;
                    if (neighbors.Count == 1)
                    {
                        next = neighbors[0]; 
                    }
                    else
                    {
                        if (prev.x == int.MinValue)
                            next = neighbors[0];
                        else
                            next = (neighbors[0] == prev) ? neighbors[1] : neighbors[0];
                    }

                    prev = curr;
                    curr = next;

                    if (curr == startV) break; // closed loop
                                               // safety break
                    if (loop.Count > edges.Count + 10) break;
                }

                if (loop.Count >= 3)
                {
                    var simplified = SimplifyColinear(loop);
                    loops.Add(simplified);
                }
            }

            if (loops.Count == 0)
            {
                borderRenderer.positionCount = 0;
                return;
            }

            List<Vector2Int> outer = loops[0];
            float bestArea = Mathf.Abs(PolygonArea(outer));
            for (int i = 1; i < loops.Count; i++)
            {
                float a = Mathf.Abs(PolygonArea(loops[i]));
                if (a > bestArea)
                {
                    bestArea = a;
                    outer = loops[i];
                }
            }

            var positions = new List<Vector3>(outer.Count);
            for (int i = 0; i < outer.Count; i++)
            {
                Vector2Int v = outer[i];
                float wx = v.x / 2f;
                float wz = v.y / 2f;
                positions.Add(new Vector3(wx, heightOffset, wz));
            }

            borderRenderer.loop = true;
            borderRenderer.positionCount = positions.Count;
            borderRenderer.SetPositions(positions.ToArray());

            borderZMin = positions.Min(p => p.z);
            borderZMax = positions.Max(p => p.z);

        }

        private void TryAddEdge(Dictionary<string, Vector2Int> edges, Vector2Int a, Vector2Int b)
        {
            Vector2Int min = (a.x < b.x || (a.x == b.x && a.y <= b.y)) ? a : b;
            Vector2Int max = (min == a) ? b : a;
            string key = $"{min.x}_{min.y}_{max.x}_{max.y}";

            if (edges.ContainsKey(key))
            {
                edges.Remove(key);
            }
            else
            {
                edges[key] = a;
            }
        }

        private List<Vector2Int> SimplifyColinear(List<Vector2Int> poly)
        {
            var res = new List<Vector2Int>();
            int n = poly.Count;
            for (int i = 0; i < n; i++)
            {
                Vector2Int prev = poly[(i - 1 + n) % n];
                Vector2Int curr = poly[i];
                Vector2Int next = poly[(i + 1) % n];

                Vector2Int v1 = new Vector2Int(curr.x - prev.x, curr.y - prev.y);
                Vector2Int v2 = new Vector2Int(next.x - curr.x, next.y - curr.y);

                long cross = (long)v1.x * v2.y - (long)v1.y * v2.x;
                if (cross != 0)
                    res.Add(curr);
            }
            return res.Count > 0 ? res : new List<Vector2Int>(poly); // fallback
        }

        private float PolygonArea(List<Vector2Int> poly)
        {
            long sum = 0;
            int n = poly.Count;
            for (int i = 0; i < n; i++)
            {
                Vector2Int a = poly[i];
                Vector2Int b = poly[(i + 1) % n];
                sum += (long)a.x * b.y - (long)b.x * a.y;
            }
            return sum * 0.5f; 
        }

        private bool IsActive(int x, int y)
        {
            if (x < 0 || y < 0 || x >= boardWidth || y >= boardHeight)
                return false;
            return ActiveBoard[IndexOf(x, y)];
        }

        private void DrawMiddleLine()
        {
            float midX = 19.5f; //middle board X

            Vector3 start = new Vector3(midX, heightOffset, borderZMin);
            Vector3 end = new Vector3(midX, heightOffset, borderZMax);

            midLineRenderer.positionCount = 2;
            midLineRenderer.SetPosition(0, start);
            midLineRenderer.SetPosition(1, end);
        }

        private int IndexOf(int x, int y)
        {
            return y * boardWidth + x;
        }
    }
}
